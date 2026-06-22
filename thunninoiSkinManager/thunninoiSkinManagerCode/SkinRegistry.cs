using System.Text.Json;
using System.Text.Json.Serialization;
using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Potions;
using thunninoiSkinManager.thunninoiSkinManagerCode.Patches;
using FileAccess = Godot.FileAccess;

namespace thunninoiSkinManager.thunninoiSkinManagerCode;

[HarmonyPatch]
public class SkinRegistry
{
    private const string savePath = "user://thunni_skin_info.json";
    private static Dictionary<ModelId, List<SkinData>> _skins = new();
    private static Dictionary<ModelId, string> _activeSkins = new();
    private static Dictionary<ModelId, int> _activePointer = new();
    private static Dictionary<ModelId, PowerSkin> _activePowers = new();
    private static Dictionary<ModelId, PotionSkin> _activePotions = new();
    private static Dictionary<ModelId, RelicSkin> _activeRelics = new();
    //private static Dictionary<string, SkinData.AudioData> _activeAudios = new(StringComparer.OrdinalIgnoreCase);
    
    // ────── Cache for power/potions/relics ──────
    private static void RefreshSkinCache()
    {
        modEntry.Logger.Info("Refreshing skin cache");
        _activePowers.Clear();
        _activePotions.Clear();
        _activeRelics.Clear();
        //_activeAudios.Clear();
        //FmodAudio.ClearReplacements();

        foreach (ModelId charId in _activeSkins.Keys)
        {
            modEntry.Logger.Info("Refreshing cache info from" + charId.Entry);
            if (!_skins.Keys.Contains(charId)) continue;
            SkinData? activeSkin = GetActiveSkin(charId);
            if (activeSkin == null || activeSkin.IsDefault)
            {
                continue;
            }

            foreach (var power in activeSkin.PowerSkinDict)
            {
               _activePowers[power.Key] = power.Value;
            }
        
            foreach (var potion in activeSkin.PotionSkinDict)
            {
                _activePotions[potion.Key] = potion.Value;
            }
        
            foreach (var relic in activeSkin.RelicSkinDict)
            {
                _activeRelics[relic.Key] = relic.Value;
            }
            
            /* Scrapped
            foreach (var audio in activeSkin.Audio)
            {
                modEntry.Logger.Info("Activating " + audio.Key);
                if (_activeAudios.ContainsKey(audio.Key)) modEntry.Logger.Warn($"{audio.Key} is already registered, will be replace with new data from {activeSkin.SkinId}");
                _activeAudios[audio.Key] = audio.Value;

                if (FmodAudio.EventExists(audio.Key))
                {
                    modEntry.Logger.Info($"Replacing {audio.Key} with {audio.Value.audioPath}");
                    FmodAudio.RegisterFileReplacement(audio.Key, audio.Value.audioPath);
                }
            }
            */
            
        }
    }
    
    // ────── Setup Skin ──────
    public static void SkinDbSetup()
    {
        Load();
        foreach (CharacterModel character in ModelDb.AllCharacters)
        {
            // database
            ModelId charId = character.Id;
            //modEntry.Logger.Info($"Found character {charName}.");
            SkinData defaultSkin = new SkinData(charId, "default", "Default");
            var skinList = new List<SkinData>();
            skinList.Add(defaultSkin.AsDefault());
            modEntry.Logger.Info($"Found {charId.Entry}, Default skin has been added to db.");
            _skins[charId] = skinList;
            
            // default active
            _activeSkins.TryAdd(charId, "default");
            modEntry.Logger.Info($"{charId} --> {_activeSkins[charId]}.");
        }
    }
    
    [HarmonyPatch(typeof(SkinRegistry), nameof(SkinRegistry.SkinDbSetup))]
    [HarmonyFinalizer]
    private static void finializeSetup()
    {
        modEntry.Logger.Info("Finalizing mod db");
        foreach (ModelId charId in _activeSkins.Keys)
        {
            //modEntry.Logger.Info(skinChar + _activeSkins[skinChar]);
            int index = GetSkinIndex(charId, _activeSkins[charId]); 
            if (index != -1) _activePointer.Add(charId, index);
            else _activePointer.Add(charId, 0);
        }
        RefreshSkinCache();
        Save();
    }
    
    public static void Register(SkinData skin)
    {
        modEntry.Logger.Info($"Trying to register {skin.SkinId} for character {skin.TargetCharacterId}");
        if (string.IsNullOrEmpty(skin.SkinId)) return;

        // Load skin data for current character
        if (!_skins.TryGetValue(skin.TargetCharacterId, out var list))
        {
            list = new List<SkinData>();
            _skins[skin.TargetCharacterId] = list;

            if (!skin.IsDefault)
            {
                SkinData defaultSkin = new SkinData(skin.TargetCharacterId, "default", "Default");
                list.Add(defaultSkin);
                modEntry.Logger.Info($"Auto-registered skin for {skin.TargetCharacterId}.");
            }
        }

        if (list.Any(s => string.Equals(s.SkinId, skin.SkinId, StringComparison.OrdinalIgnoreCase)))
        {
                modEntry.Logger.Warn($"{skin.SkinId} is already registered for {skin.TargetCharacterId}");
                return;
        }

        list.Add(skin);
        modEntry.Logger.Info($"{skin.SkinId} has been registered for {skin.TargetCharacterId}");
        foreach (var skinData in _skins[skin.TargetCharacterId])
        {
            modEntry.Logger.Info($"[{skinData.SkinId}] {skinData.SkinId}");
        }
}

    // ────── Managing Save files ──────
    public static void Load()
    {
        try
        {
            if (!FileAccess.FileExists(savePath)) return;
            using FileAccess saveFile = FileAccess.Open(savePath, FileAccess.ModeFlags.Read);
            if (saveFile == null) return;
            var saveData = JsonSerializer.Deserialize<Dictionary<string, string>>(saveFile.GetAsText());
            if (saveData == null) return;
            foreach (var (key, value) in saveData)
            {
                modEntry.Logger.Info($"{key} --> {value}");
                _activeSkins[ModelId.Deserialize(key)] = value;
            }
            modEntry.Logger.Info("Loaded saved skins.");
        } catch (Exception e)
        {
            modEntry.Logger.Error("Load saved skins failed -> " + e.Message);
        }
    }

    public static void Save()
    {
        try
        {
            Dictionary<string, string> saveData = new Dictionary<string, string>();
            foreach (var (key, value) in _activeSkins)
            {
                saveData[key.ToString()] = value;
            }
            string json = JsonSerializer.Serialize(saveData);
            using FileAccess saveFile = FileAccess.Open(savePath, FileAccess.ModeFlags.Write);
            saveFile?.StoreString(json);
            modEntry.Logger.Info("Saved skins.");
        } catch (Exception e)
        {
            modEntry.Logger.Error("Save failed -> " + e.Message);
        }
    }
    
    // ────── Utilities ──────
    public static SkinData? GetActiveSkin(ModelId characterId)
    {
        if (!_skins.ContainsKey(characterId)) return null;
        int index = _activePointer[characterId];
        return _skins[characterId][index];
    }
    
    public static int GetSkinIndex(ModelId characterId, string skindId)
    {
        if (!_skins.ContainsKey(characterId)) return -1;
        return _skins[characterId].FindIndex(s => s.SkinId == skindId);
    }
    
    public static List<SkinData> GetAllSkins(ModelId characterId) => _skins[characterId];
    //public static List<string> GetAllCharacterIds() => _skins.Select(s => s.Key).ToList();

    public static bool IsUsingSkin(ModelId characterId, string skinId) => _activeSkins[characterId] == skinId;
    
    // Change skin
    public static void SetActiveSkin(ModelId characterId, string skinId)
    {
        if (!_skins.ContainsKey(characterId)) return;
        _activeSkins[characterId] = skinId;
        _activePointer[characterId] = GetSkinIndex(characterId, skinId);
        RefreshSkinCache();
        Save();
    }

    public static void SetActiveSkin(ModelId characterId, int skinIndex)
    {
        if (skinIndex >= _skins[characterId].Count) return;
        _activeSkins[characterId] = _skins[characterId][skinIndex].SkinId;
        _activePointer[characterId] = skinIndex;
        RefreshSkinCache();
        Save();
    }

    public static void CycleNext(ModelId characterId)
    {
        int currentIndex = _activePointer[characterId];
        int maxSkin = _skins[characterId].Count;
        SetActiveSkin(characterId, (currentIndex + 1) % maxSkin);
        modEntry.Logger.Info("Character registered to : " + characterId);
        foreach (var skin in _skins[characterId])
        {
            modEntry.Logger.Info(" - " + skin.SkinId);
        }
    }

    public static void CyclePrevious(ModelId characterId)
    {
        int currentIndex = _activePointer[characterId];
        int maxSkin = _skins[characterId].Count;
        SetActiveSkin(characterId, (currentIndex - 1 + maxSkin) % maxSkin);
    }
    
    // ────── Getters ──────
    public static Dictionary<ModelId, string> GetAllActiveSkins() => _activeSkins;
    
    internal static bool Resolve<T>(ModelId characterId, Func<SkinData, T?> selector, out T? value)
    {
        if (!_skins.ContainsKey(characterId))
        {
            value = default;
            return false;
        }
        SkinData? skinData = GetActiveSkin(characterId);
        if (skinData == null || skinData.IsDefault)
        {
            value = default;
            return false;
        }
        value = selector(skinData);
        return true;
    }

    internal static Color? ResolveColor(ModelId characterId, Func<SkinData, Color?> selector)
    {
        if (Resolve<Color?>(characterId, selector, out var color)) return color;
        return null;
    }

    internal static string? ResolvePath(ModelId characterId, Func<SkinData, string?> selector)
    {
        if (Resolve<string?>(characterId, selector, out var path)) return path;
        return null;
    }

    internal static Texture2D? ResolveTexture(ModelId characterId, Func<SkinData, string?> selector)
    {
        if (Resolve<string?>(characterId, selector, out var texturePath))
        {
            if(texturePath != null) return PreloadManager.Cache.GetTexture2D(texturePath);
        }
        return null;
    }

    /*
    internal static SkinData.AudioData? sfxResolve(string characterId, string audioKey)
    {
        if (GetActiveSkin(characterId).Audio.TryGetValue(audioKey, out SkinData.AudioData? sfxPath))
        {
            if (!Path.Exists(sfxPath.audioPath))
            {
                modEntry.Logger.Warn($"Cannot find {sfxPath} for {characterId}!");
                return null;
            }
            return sfxPath;
        };
        return null;
    } 
    */

    internal static PowerSkin? ResolvePower(ModelId powerId)
    {
        if (_activePowers.TryGetValue(powerId, out PowerSkin? powerSkin)) return powerSkin;
        return null;
    }
       

    internal static PotionSkin? ResolvePotion(ModelId potionId)
    {
        if (_activePotions.TryGetValue(potionId, out PotionSkin? potionSkin)) return potionSkin;
        return null;
    }
    
    internal static RelicSkin? ResolveRelic(ModelId relicId)
    {
        if(_activeRelics.TryGetValue(relicId, out var relicSkin)) return relicSkin;
        return null;
    }

    internal static OrbSkin? ResolveOrb(ModelId orbId)
    {
        if (Resolve<Dictionary<ModelId, OrbSkin>>(ModelDb.Character<Defect>().Id, s => s.OrbSkinDict,
                out Dictionary<ModelId, OrbSkin>? orbSkinDict))
        {
            if (orbSkinDict != null && orbSkinDict.TryGetValue(orbId, out OrbSkin? orbSkin)) return orbSkin;
        }
        return null;
    }

    internal static bool ResolveConfig(ModelId charId, string key, bool defaultValue = true)
    {
        if (Resolve<bool>(charId, s => s.IsConfigEnabled(key, defaultValue), out bool configEnabled))
            return configEnabled;
        return defaultValue;
    }
    
}