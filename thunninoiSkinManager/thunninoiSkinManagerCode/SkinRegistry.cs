using System.Text.Json;
using System.Text.Json.Serialization;
using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using FileAccess = Godot.FileAccess;

namespace thunninoiSkinManager.thunninoiSkinManagerCode;

[HarmonyPatch]
public class SkinRegistry
{
    private const string savePath = "user://thunni_skin_info.json";
    private static Dictionary<string, List<SkinData>> _skins = new(StringComparer.OrdinalIgnoreCase);
    private static Dictionary<string, string> _activeSkins = new(StringComparer.OrdinalIgnoreCase);
    private static Dictionary<string, int> _activePointer = new(StringComparer.OrdinalIgnoreCase);
    private static Dictionary<Type, SkinData.PowerSkinData> _activePowers = new();
    private static Dictionary<Type, SkinData.PotionSkinData> _activePotions = new();
    private static Dictionary<Type, SkinData.RelicSkinData> _activeRelics = new();
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

        foreach (string charId in _activeSkins.Keys)
        {
            modEntry.Logger.Info("Checkign cache info for " + charId);
            if (!_skins.Keys.Contains(charId)) continue;
            SkinData? activeSkin = GetActiveSkin(charId);
            if (activeSkin == null || activeSkin.IsDefault)
            {
                continue;
            }

            foreach (var power in activeSkin.Powers)
            {
                modEntry.Logger.Info("Activating " + power.Key.Name);
                if (_activePowers.ContainsKey(power.Key)) modEntry.Logger.Warn($"{power.Key} is already registered, will be replace with new data from {activeSkin.SkinId}");
                _activePowers[power.Key] = power.Value;
            }
        
            foreach (var potion in activeSkin.Potions)
            {
                modEntry.Logger.Info("Activating " + potion.Key.Name);
                if (_activePotions.ContainsKey(potion.Key)) modEntry.Logger.Warn($"{potion.Key} is already registered, will be replace with new data from {activeSkin.SkinId}");
                _activePotions[potion.Key] = potion.Value;
            }
        
            foreach (var relic in activeSkin.Relics)
            {
                modEntry.Logger.Info("Activating " + relic.Key.Name);
                if (_activeRelics.ContainsKey(relic.Key)) modEntry.Logger.Warn($"{relic.Key} is already registered, will be replace with new data from {activeSkin.SkinId}");
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
            string charName = character.Id.Entry.ToLower();
            //modEntry.Logger.Info($"Found character {charName}.");
            SkinData defaultSkin = new SkinData(charName, "default", "Default");
            var skinList = new List<SkinData>();
            skinList.Add(defaultSkin.AsDefault());
            modEntry.Logger.Info($"Found {charName}, Default skin has been added to db.");
            _skins[charName] = skinList;
            
            // default active
            _activeSkins.TryAdd(charName, "default");
            modEntry.Logger.Info($"{charName} --> {_activeSkins[charName]}.");
        }
    }
    
    [HarmonyPatch(typeof(SkinRegistry), nameof(SkinRegistry.SkinDbSetup))]
    [HarmonyFinalizer]
    private static void finializeSetup()
    {
        modEntry.Logger.Info("Finalizing mod db");
        foreach (string skinChar in _activeSkins.Keys)
        {
            //modEntry.Logger.Info(skinChar + _activeSkins[skinChar]);
            int index = GetSkinIndex(skinChar, _activeSkins[skinChar]); 
            if (index != -1) _activePointer.Add(skinChar, index);
            else _activePointer.Add(skinChar, 0);
        }
        RefreshSkinCache();
        Save();
    }
    
    public static void Register(SkinData skin, bool replace = false)
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
            if (!replace)
            {
                modEntry.Logger.Warn($"{skin.SkinId} is already registered for {skin.TargetCharacterId}");
                return;
            }
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
            foreach (var activeSkin in saveData)
            {
                modEntry.Logger.Info($"{activeSkin.Key} --> {activeSkin.Value}");
                _activeSkins[activeSkin.Key] = activeSkin.Value;
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
            string json = JsonSerializer.Serialize(_activeSkins);
            using FileAccess saveFile = FileAccess.Open(savePath, FileAccess.ModeFlags.Write);
            saveFile?.StoreString(json);
            modEntry.Logger.Info("Saved skins.");
        } catch (Exception e)
        {
            modEntry.Logger.Error("Save failed -> " + e.Message);
        }
    }
    
    // ────── Utilities ──────
    public static SkinData? GetActiveSkin(string characterId)
    {
        if (!_skins.ContainsKey(characterId)) return null;
        int index = _activePointer[characterId];
        return _skins[characterId][index];
    }
    
    public static int GetSkinIndex(string characterId, string skindId)
    {
        if (!_skins.ContainsKey(characterId)) return -1;
        for (int i = 0; i < _skins[characterId].Count; i++)
        {
            modEntry.Logger.Info($"Checking {_skins[characterId][i].SkinId}");
            if (string.Equals(_skins[characterId][i].SkinId, skindId, StringComparison.OrdinalIgnoreCase)) return i;
        }
        modEntry.Logger.Warn($"Cannot find {skindId} for character {characterId}!");
        return -1;
    }
    
    public static List<SkinData> GetAllSkins(string characterId) => _skins[characterId];
    public static List<string> GetAllCharacterIds() => _skins.Select(s => s.Key).ToList();

    public static bool IsUsingSkin(string characterId, string skinId) => _activeSkins[characterId] == skinId;
    
    // Change skin
    public static void SetActiveSkin(string characterId, string skinId)
    {
        if (!_skins.ContainsKey(characterId)) return;
        _activeSkins[characterId] = skinId;
        _activePointer[characterId] = GetSkinIndex(characterId, skinId);
        RefreshSkinCache();
        Save();
    }

    public static void SetActiveSkin(string characterId, int skinIndex)
    {
        if (skinIndex >= _skins[characterId].Count) return;
        _activeSkins[characterId] = _skins[characterId][skinIndex].SkinId;
        _activePointer[characterId] = skinIndex;
        RefreshSkinCache();
        Save();
    }

    public static void CycleNext(string characterId)
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

    public static void CyclePrevious(string characterId)
    {
        int currentIndex = _activePointer[characterId];
        int maxSkin = _skins[characterId].Count;
        SetActiveSkin(characterId, (currentIndex - 1 + maxSkin) % maxSkin);
    }
    
    // ────── Getters ──────
    internal static string? PathResolve(string characterId, Func<SkinData, string?> selector)
    {
        if (!GetAllCharacterIds().Contains(characterId)) return null;
        SkinData? skin = GetActiveSkin(characterId);
        if (skin == null || skin.SkinId == "default") return null;
        
        string? val = selector(skin);
        if (string.IsNullOrEmpty(val)) return null;

        if (!ResourceLoader.Exists(val))
        {
            modEntry.Logger.Warn($"Cannot find {val} for {skin.SkinId} ({characterId})!");
            return null;
        }
        return val;
    }

    internal static Color? colResolve(string characterId, Func<SkinData, Color?> selector)
    {
        if (!GetAllCharacterIds().Contains(characterId)) return null;
        SkinData? skin = GetActiveSkin(characterId);
        if (skin == null || skin.IsDefault) return null;
        return selector(skin);
    }

    internal static Texture2D textureResolve(string characterId, Func<SkinData, string?> selector)
    {
        if (!GetAllCharacterIds().Contains(characterId)) return null;
        string? path = PathResolve(characterId, selector);
        if (string.IsNullOrWhiteSpace(path)) return null;
        Texture2D loadedTexture = ResourceLoader.Load<Texture2D>(path, null, ResourceLoader.CacheMode.Reuse);
        return loadedTexture;
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

    internal static SkinData.PowerSkinData? PowerResolve(PowerModel powerObj) =>
        _activePowers.TryGetValue(powerObj.GetType(), out var data) ? data : null;

    internal static SkinData.PotionSkinData? PotionResolve(PotionModel potionObj)
    {
        modEntry.Logger.Info("Resolve has been called for " + potionObj.GetType().Name);
        foreach (SkinData.PotionSkinData potion in _activePotions.Values)
        {
            modEntry.Logger.Info(" - " + potion.SpritePath);
        }
        return _activePotions.TryGetValue(potionObj.GetType(), out var data) ? data : null;
    }

    internal static SkinData.RelicSkinData? RelicResolve(RelicModel relicObj)
    {
        modEntry.Logger.Info("Resolve has been called for " + relicObj.GetType().Name);
        foreach (SkinData.RelicSkinData relic in _activeRelics.Values)
        {
            modEntry.Logger.Info(" - " + relic.IconPath);
        }
        return _activeRelics.TryGetValue(relicObj.GetType(), out var data) ? data : null;
    }

    internal static SkinData.OrbSkinData? OrbResolve(string orbId)
    {
        SkinData? defectSkin = GetActiveSkin("defect");
        if (defectSkin.IsDefault) return null;
        if (defectSkin.OrbSkins.TryGetValue(orbId, out var orbSkin)) return orbSkin;
        return null;
    }

    internal static bool resolveConfig(string charId, string key)
    {
        SkinData? activeSkin = GetActiveSkin(charId);
        if (activeSkin == null || activeSkin.IsDefault) return true;
        return activeSkin.IsConfigEnabled(key);
    }
    
    internal static T? Resolve<T>(string characterId, string key) where T : class
    {
        if (!GetAllCharacterIds().Contains(characterId)) return null;
        SkinData? currentSkin = GetActiveSkin(characterId);
        if (currentSkin.IsDefault) return null;
        return currentSkin.GetCustom<T>(key);
    }

    internal static object? Resolve(string characterId, string key)
    {
        if (!GetAllCharacterIds().Contains(characterId)) return null;
        SkinData? currentSkin = GetActiveSkin(characterId);
        if (currentSkin.IsDefault) return null;
        return currentSkin.GetCustom(key);
    }
}