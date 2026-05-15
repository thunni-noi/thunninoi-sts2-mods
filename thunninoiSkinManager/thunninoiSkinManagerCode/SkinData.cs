using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace thunninoiSkinManager.thunninoiSkinManagerCode;

public class SkinData(string targetCharacterId, string skinId, string skinName)
{
    public string TargetCharacterId { get; } = targetCharacterId.ToUpperInvariant();
    public string SkinId { get; } = skinId;
    public string SkinName { get; } = skinName;
    public bool IsDefault { get; private set; } = false;
 
    internal string? CombatVisuals { get; private set; }
    internal string? MerchantAnim { get; private set; }
    internal string? RestSiteAnim { get; private set; }
    internal string? CharacterSelectBg { get; private set; }
    internal string? CharacterSelectPortrait { get; private set; }
    internal string? CharacterSelectTransition { get; private set; }
    internal string? CharacterIcon { get; private set; }
    internal string? CharacterIconOutline { get; private set; }
    internal string? IconScene { get; private set; }
    internal string? MapMarker { get; private set; }
    internal string? CardFrameMaterial { get; private set; }
    internal string? CardTrail { get; private set; }
    internal string? EnergyIcon { get; private set; }
    internal string? EnergyLayer1 { get; private set; }
    internal string? EnergyLayer2 { get; private set; }
    internal string? EnergyLayer3 { get; private set; }
    internal string? EnergyLayer4 { get; private set; }
    internal string? EnergyLayer5 { get; private set; }
    internal Color? EnergyOutlineColor { get; private set; }
    internal Color? EnergyLabelOutlineColor { get; private set; }
    internal string? HandPoint { get; private set; }
    internal string? HandRock { get; private set; }
    internal string? HandPaper { get; private set; }
    internal string? HandScissors { get; private set; }
    internal Color? ShivTintColor { get; private set; }
    internal string? PreviewSkeletonData { get; private set; }
    
    internal Dictionary<string, OrbSkinData> OrbSkins { get; } = new();
    internal Dictionary<Type, PowerSkinData> Powers { get; } = new();
    internal Dictionary<Type, PotionSkinData> Potions { get; } = new();
    internal Dictionary<Type, RelicSkinData> Relics { get; } = new();
    //internal Dictionary<string, AudioData> Audio { get; } = new(StringComparer.OrdinalIgnoreCase);
 
    private readonly Dictionary<string, object> _customData = new();
    
    // Config Loader
    public static class SkinConfigKey
    {
        public const string UseCardFrame = "UseCardFrame";
        public const string UseEnergy = "UseEnergy";
        public const string  UseHands = "UseHands";
        public const string SilentRecolorShiv = "SilentRecolorShiv";
        public const string  UseDefectOrbs = "UseDefectOrbs";
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
 
    public SkinData RegisterVisuals(string combatScene,
        string? merchantScene = null, string? restSiteScene = null)
    {
        CombatVisuals = combatScene;
        MerchantAnim = merchantScene;
        RestSiteAnim = restSiteScene;
        return this;
    }
 
    public SkinData RegisterCharacterSelect(string? bg = null,
        string? icon = null, string? transition = null)
    {
        CharacterSelectBg = bg;
        CharacterSelectPortrait = icon;
        CharacterSelectTransition = transition;
        return this;
    }
 
    public SkinData RegisterIcon(string? iconPath, string? outlinePath = null,
        string? iconScene = null, string? mapMarker = null)
    {
        CharacterIcon = iconPath;
        CharacterIconOutline = outlinePath;
        IconScene = iconScene;
        MapMarker = mapMarker;
        return this;
    }
 
    public SkinData RegisterCardMaterial(string? frameMaterial = null,
        string? cardTrail = null)
    {
        CardFrameMaterial = frameMaterial;
        CardTrail = cardTrail;
        return this;
    }
 
    public SkinData RegisterEnergy(string? energyIcon = null,
        string? layer1 = null, string? layer2 = null, string? layer3 = null, string? layer4 = null, string? layer5 = null)
    {
        EnergyIcon = energyIcon;
        EnergyLayer1 = layer1;
        EnergyLayer2 = layer2;
        EnergyLayer3 = layer3;
        EnergyLayer4 = layer4;
        EnergyLayer5 = layer5;
        return this;
    }
 
    public SkinData RegisterEnergyColor(Color? outlineColor = null,
        Color? labelOutlineColor = null)
    {
        EnergyOutlineColor = outlineColor;
        EnergyLabelOutlineColor = labelOutlineColor;
        return this;
    }
 
    public SkinData RegisterHands(string? point = null, string? rock = null,
        string? paper = null, string? scissors = null)
    {
        HandPoint = point;
        HandRock = rock;
        HandPaper = paper;
        HandScissors = scissors;
        return this;
    }
 
    public SkinData RegisterShivTint(Color color)
    {
        ShivTintColor = color;
        return this;
    }
 
    public SkinData RegisterOrb(string orbId, string orbScene, string orbIcon, Color? orbDarkenedColor)
    {
        if (orbDarkenedColor == null) orbDarkenedColor = new Color("454545");
        OrbSkins[orbId.ToLowerInvariant()] = new OrbSkinData(orbScene, orbIcon, orbDarkenedColor.Value);
        return this;
    }

    public SkinData RegisterPower<T>(string powerIconPath, string? powerBigIconPath)
    {
        if (powerBigIconPath == null) powerBigIconPath = powerIconPath;
        Powers[typeof(T)] = new PowerSkinData(powerIconPath, powerBigIconPath);
        return this;
    }
    
    public SkinData RegisterPotion<T>(string spritePath,
        string? outlinePath = null, string? thrownSpritePath = null)
    {
        modEntry.Logger.Info("Potion register has been called for " + typeof(T));
        Potions[typeof(T)] = new PotionSkinData(spritePath, outlinePath, thrownSpritePath);
        foreach (var potionData in Potions)
        {
            modEntry.Logger.Info(potionData.Key + ": " + potionData.Value.SpritePath);
        }
        return this;
    }
    
 
    public SkinData RegisterRelic<T>(string iconPath,
        string? outlinePath = null, string? bigIconPath = null)
    {
        modEntry.Logger.Info("Relic register has been called for " + typeof(T));
        if (string.IsNullOrWhiteSpace(bigIconPath)) bigIconPath = iconPath;
        Relics[typeof(T)] = new RelicSkinData(iconPath, outlinePath, bigIconPath);
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
 
    internal bool HasAsset(Func<SkinData, string?> selector)
    {
        string? val = selector(this);
        return !string.IsNullOrWhiteSpace(val);
    }
 
    internal bool HasColorAsset(Func<SkinData, Color?> selector)
        => selector(this).HasValue;

    internal record AudioData(string audioPath, float volume, float pitch);
    internal record PowerSkinData(string PowerIcon, string? PowerBigIcon);
    
    internal record OrbSkinData(string OrbScenePath, string OrbIconPath, Color OrbDarkenedColor);
    
    internal record PotionSkinData(string SpritePath, string? OutlinePath,
        string? ThrownSpritePath);
 
    internal record RelicSkinData(string IconPath, string? OutlinePath,
        string? BigIconPath);
    
}
 
public static class SkinDataExtensions
{
    public static void Let(this SkinData skin, Action<SkinData> action)
        => action(skin);
}
