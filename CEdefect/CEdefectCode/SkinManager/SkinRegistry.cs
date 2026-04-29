using System.Resources;
using System.Text.Json;
using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves;
using FileAccess = Godot.FileAccess;

namespace CEdefect.CEdefectCode.SkinManager;

public class SkinRegistry
{
    private const string SkinStatePath = "user://civilight_eterna_skin.json";

    private static readonly List<SkinDefinition> _skins = new();
    private static int _currentSkinIndex = 0;

    public static string CharacterId { get; } = "DEFECT";

    public static IReadOnlyList<SkinDefinition> AllSkins => _skins;
    public static SkinDefinition? CurrentSkin => _skins.Count > 0 ? _skins[_currentSkinIndex] : null;
    public static int CurrentSkinIndex => _currentSkinIndex;

    public static void RegisterSkin(SkinDefinition skin)
    {
        if (skin == null || string.IsNullOrWhiteSpace(skin.SkinId)) return;
        // duplicate skin prevent??
        if (_skins.Exists(x => string.Equals(x.SkinId, skin.SkinId, StringComparison.OrdinalIgnoreCase))) return;
        _skins.Add(skin);
    }

    public static void SetActiveSkin(int index)
    {
        if (index < 0 || index >= _skins.Count) return;
        _currentSkinIndex = index;
        //Save();
    }

    public static void SetActiveSkin(string skinId)
    {
        int index=  _skins.FindIndex(x => string.Equals(x.SkinId, skinId, StringComparison.OrdinalIgnoreCase));
        if (index >= 0) SetActiveSkin(index);
    }

    public static void CycleSkin()
    {
        if (_skins.Count == 0) return;
        SetActiveSkin((_currentSkinIndex + 1) % _skins.Count);
        Save();
    }
    
    // Resolve func
    public static string? Resolve(Func<SkinDefinition, string?> selector, string? defaultValue = null)
    {
        string? value = CurrentSkin != null ? selector(CurrentSkin) : null;
        if (string.IsNullOrWhiteSpace(value) || (!ResourceLoader.Exists(value))) return defaultValue;
        return value;
    }

    public static Color? ResolveColor(Func<SkinDefinition, Color?> selector)
    {
        return CurrentSkin != null ? selector(CurrentSkin) : null;
    }

    public static PackedScene? ResolveScene(Func<SkinDefinition, string?> selector)
    {
        string? path = Resolve(selector);
        if (string.IsNullOrWhiteSpace(path)) return null;
        if (!ResourceLoader.Exists(path, "")) return null;
        return ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Reuse);
    }

    public static void Load()
    {
        try
        {
            if (!FileAccess.FileExists(SkinStatePath))
            {
                Log.Error("[CE] File does not exists");
                return;
            }
            using FileAccess file = FileAccess.Open(SkinStatePath, FileAccess.ModeFlags.Read);
            if (file == null) return;
            string json = file.GetAsText();
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            if (data != null && data.TryGetValue("activeSkinId", out string? skinId)) SetActiveSkin(skinId);

        }
        catch (Exception e)
        {
            Log.Error("[CE_defect] SkinRegistry load failed : " + e.Message);
        }
    }
    public static void Save()
    {
        try
        {
            string json = JsonSerializer.Serialize(new Dictionary<string, string>
            {
                { "activeSkinId", CurrentSkin?.SkinId ?? "" }
            });
            using FileAccess file = FileAccess.Open(SkinStatePath, FileAccess.ModeFlags.Write);
            file?.StoreString(json);
        }
        catch (Exception e)
        {
            Log.Error("[CE_defect] SkinRegistry save failed : " + e.Message);
        }
    }
    
    // --------------------------------------------------------------------------------
    
    public static void RegisterAll()
    {
        SkinRegistry.RegisterSkin(new SkinDefinition
        {
            SkinId = "default",
            SkinName = "Default",
            CombatScene = "res://CEdefect/scenes/default/ce_combat.tscn",
            MerchantScene = "res://CEdefect/scenes/default/ce_merchant.tscn",
            RestScene = "res://CEdefect/scenes/default/ce_rest.tscn",
            CharacterSelectBg = "res://CEdefect/scenes/default/ce_default_charsel.tscn",
            CharacterSelectPortrait = "res://CEdefect/assets/shared/ce_portrait.png",
            CharacterIcon = "res://CEdefect/assets/default/ce_default_icon.png",
            CharacterIconOutline = "res://CEdefect/assets/default/ce_default_icon_outline.png",
            CharacterIconScene = "res://CEdefect/scenes/default/ce_default_icon.tscn",
            CharacterMapIcon = "res://CEdefect/assets/shared/ce_map_icon.png",
            ArmPointing = "res://CEdefect/assets/default/arms/ce_default_point.png",
            ArmRock = "res://CEdefect/assets/default/arms/ce_default_rock.png",
            ArmPaper = "res://CEdefect/assets/default/arms/ce_default_paper.png",
            ArmScissors = "res://CEdefect/assets/default/arms/ce_default_scissor.png"
        });
       
        SkinRegistry.RegisterSkin(new SkinDefinition
        {
            SkinId = "epoque",
            SkinName = "Condolences",
            CombatScene = "res://CEdefect/scenes/epoque/ce_epoque_combat.tscn",
            MerchantScene = "res://CEdefect/scenes/epoque/ce_epoque_merchant.tscn",
            RestScene = "res://CEdefect/scenes/epoque/ce_epoque_rest.tscn",
            CharacterSelectBg = "res://CEdefect/scenes/epoque/charselect/ce_epoque_charsel.tscn",
            CharacterSelectPortrait = "res://CEdefect/assets/shared/ce_portrait.png",
            CharacterIcon = "res://CEdefect/assets/epoque/ce_epoque_icon.png",
            CharacterIconOutline = "res://CEdefect/assets/epoque/ce_epoque_icon_outline.png",
            CharacterIconScene = "res://CEdefect/scenes/epoque/ce_epoque_icon.tscn",
            CharacterMapIcon = "res://CEdefect/assets/shared/ce_map_icon.png",
            ArmPointing = "res://CEdefect/assets/epoque/arms/ce_epoque_point.png",
            ArmRock = "res://CEdefect/assets/epoque/arms/ce_epoque_rock.png",
            ArmPaper = "res://CEdefect/assets/epoque/arms/ce_epoque_paper.png",
            ArmScissors = "res://CEdefect/assets/epoque/arms/ce_epoque_scissor.png"
        });
    }
}