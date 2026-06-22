using Godot;
using MegaCrit.Sts2.Core.Models;
using thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

namespace thunninoiSkinManager.thunninoiSkinManagerCode;

public class SkinData(ModelId targetCharacterId, string skinId, string skinName)
{
    public ModelId TargetCharacterId { get; } = targetCharacterId;
    public string SkinId { get; } = skinId;
    public string SkinName { get; } = skinName;
    public bool IsDefault { get; private set; } = false;

    internal CharacterSkin? CharacterSkinData { get; private set; } = null;

    internal SkinData? FallbackSkin { get; private set; } = null;

    internal Color? ShivTintColor { get; private set; }
    internal string? PreviewSkeletonData { get; private set; }

    internal Dictionary<ModelId, OrbSkin> OrbSkinDict { get; } = new();
    internal Dictionary<ModelId, PowerSkin> PowerSkinDict { get; } = new();
    internal Dictionary<ModelId, PotionSkin> PotionSkinDict { get; } = new();
    internal Dictionary<ModelId, RelicSkin> RelicSkinDict { get; } = new();
    //internal Dictionary<string, AudioData> Audio { get; } = new(StringComparer.OrdinalIgnoreCase);

    private readonly Dictionary<string, object> _customData = new();

    // Config Loader -- Note that this is basic stuff, if needed config can be added more via RegisterConfig and can be read via IsConfigEnabled
    public static class SkinConfigKey
    {
        public const string UseCardFrame = "UseCardFrame";
        public const string UseEnergy = "UseEnergy";
        public const string UseHands = "UseHands";
        public const string SilentRecolorShiv = "SilentRecolorShiv";
        public const string UseDefectOrbs = "UseDefectOrbs";
        public const string UseRegentBlade = "UseRegentBlade";
    }

    private readonly Dictionary<string, Func<bool>> _configLoaded = new();

    public SkinData RegisterConfig(string key, Func<bool> configVar)
    {
        _configLoaded[key] = configVar;
        return this;
    }

    public bool IsConfigEnabled(string key, bool defaultVar = true)
    {
        if (_configLoaded.TryGetValue(key, out var condition)) return condition();
        return defaultVar;
    }

    // mark default skin
    public SkinData AsDefault()
    {
        IsDefault = true;
        return this;
    }

    public SkinData RegisterCharacter(CharacterSkin characterSkin)
    {
        CharacterSkinData = characterSkin;
        return this;
    }

    public SkinData RegisterShivTint(Color color)
    {
        ShivTintColor = color;
        return this;
    }

    public SkinData RegisterOrb(OrbSkin orbSkinData)
    {
        if (orbSkinData.TargetOrbId is ModelId targerId) OrbSkinDict[targerId] = orbSkinData;
        return this;
    }

    public SkinData RegisterPower(PowerSkin powerSkinData)
    {
        if (powerSkinData.TargetPowerId is ModelId targetId) PowerSkinDict[targetId] = powerSkinData;
        return this;
    }

    public SkinData RegisterPotion(PotionSkin potionSkinData)
    {
        if (potionSkinData.TargetPotionId is ModelId targetId) PotionSkinDict[targetId] = potionSkinData;
        return this;
    }


    public SkinData RegisterRelic(RelicSkin relicSkinData)
    {
        if (relicSkinData.TargetRelicId is ModelId targetId) RelicSkinDict[targetId] = relicSkinData;
        return this;
    }

    /* Scrapped, as baselib currently has no stable audio support (FModAudio is deprecated)
    public SkinData RegisterSfx(string key, string sfxPath, float volume = 1.0f, float pitch = 1.0f)
    {
        //Audio[key] = new AudioData(sfxPath, volume, pitch);
        //if (key == "charIntro") Audio[$"event:/sfx/characters/{TargetCharacterId.ToLower()}/{TargetCharacterId.ToLower()}_select"] = new AudioData(sfxPath, volume, pitch);
        return this;
    }
    */

    public SkinData RegisterCustom(string key, object value)
    {
        if (!string.IsNullOrWhiteSpace(key))
            _customData[key] = value;
        return this;
    }

    public T? GetCustom<T>(string key) where T : class
    {
        if (_customData.TryGetValue(key, out object? val) && val is T typed)
            return typed;
        return null;
    }

    public object? GetCustom(string key)
        => _customData.TryGetValue(key, out object? val) ? val : null;
    
}