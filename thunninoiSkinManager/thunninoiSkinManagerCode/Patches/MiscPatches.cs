using System.Net.Mime;
using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

[HarmonyPatch]
public class MiscPatches
{
    // Silent's shiv color
    [HarmonyPatch(typeof(NShivThrowVfx), "ApplyTint")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool ShivTintRecolor(ref Color tint)
    {
        Color? color = SkinRegistry.colResolve("silent", s => s.ShivTintColor);
        if (color == null) return true;
        tint = color.Value;
        return false;
    }
    
    // Defect's orb
    [HarmonyPatch(typeof(OrbModel), "CreateSprite")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool OrbReplace(OrbModel __instance, ref Node2D __result)
    {
        if (!SkinRegistry.resolveConfig("defect", SkinData.SkinConfigKey.UseDefectOrbs)) return true;
        string orbId = __instance.Id.Entry.ToLower();
        //modEntry.Logger.Info($"Orb {orbId} recreated");
        if (string.IsNullOrWhiteSpace(orbId)) return true;
        SkinData.OrbSkinData? orbData = SkinRegistry.OrbResolve(orbId);
        if (orbData == null) return true;
        modEntry.Logger.Info($"Found {orbData.OrbScenePath} for orb {orbId}!");
        string? orbScenePath = orbData?.OrbScenePath;
        if (string.IsNullOrWhiteSpace(orbScenePath)) return true;
        if (!ResourceLoader.Exists(orbScenePath))
        {
            modEntry.Logger.Warn($"Could not find {orbScenePath} for orb {orbId}!");
            return true;
        }
        
        PackedScene newOrb = ResourceLoader.Load<PackedScene>(orbScenePath, null, ResourceLoader.CacheMode.Reuse);
        Node2D orbNode = newOrb.Instantiate<Node2D>();
        Node orbSprite = orbNode.GetNode("OrbRender");
        new MegaSprite(orbSprite.GetNode("OrbSprite")).GetAnimationState().SetAnimation("idle_loop");
        __result = orbNode;
        return false;
    }

    [HarmonyPatch(typeof(OrbModel), "get_IconPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool OrbIconReplace(OrbModel __instance, ref string __result)
    {
        if (!SkinRegistry.resolveConfig("defect", SkinData.SkinConfigKey.UseDefectOrbs)) return true;
        string orbId = __instance.Id.Entry.ToLower();
        SkinData.OrbSkinData? orbData = SkinRegistry.OrbResolve(orbId);
        string orbIconPath = orbData?.OrbIconPath;
        if (string.IsNullOrWhiteSpace(orbIconPath)) return true;
        if (!ResourceLoader.Exists(orbIconPath))
        {
            modEntry.Logger.Warn($"Could not find {orbIconPath} for orb {orbId}!");
            return true;
        }
        __result = orbIconPath;
        return false;
    }

    internal static Texture2D? LoadTexture(string? texturePath)
    {
        if (string.IsNullOrWhiteSpace(texturePath)) return null;
        if (!ResourceLoader.Exists(texturePath))
        {
            modEntry.Logger.Warn($"Path {texturePath} is not found!");
            return null;
        }
        return ResourceLoader.Load<Texture2D>(texturePath, null, ResourceLoader.CacheMode.Reuse);
    }
    
    // Powers
    [HarmonyPatch(typeof(PowerModel), "get_Icon")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool PowerIconReplace(PowerModel __instance, ref Texture2D __result)
    {
        SkinData.PowerSkinData? powerSkin = SkinRegistry.PowerResolve(__instance);
        if (powerSkin == null) return true;
        Texture2D? powerIconTexture = LoadTexture(powerSkin.PowerIcon);
        if (powerIconTexture == null) return true;
        __result = powerIconTexture;
        return false;
    }

    [HarmonyPatch(typeof(PowerModel), "get_BigIcon")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool PowerBigIconReplace(PowerModel __instance, ref Texture2D __result)
    {
        SkinData.PowerSkinData? powerSkin = SkinRegistry.PowerResolve(__instance);
        if (powerSkin == null) return true;
        Texture2D? powerBigIconTexture = LoadTexture(powerSkin.PowerBigIcon);
        if (powerBigIconTexture == null) return true;
        __result = powerBigIconTexture;
        return false;
    }
    
    // Potions
    [HarmonyPatch(typeof(NPotion), "Reload")]
    [HarmonyPostfix]
    private static void PotionReplace(NPotion __instance)
    {
        if (!__instance.IsNodeReady()) return;
        SkinData.PotionSkinData? potionSkin = SkinRegistry.PotionResolve(__instance.Model);
        if (potionSkin == null) return;
        modEntry.Logger.Info("Potion Reload " + potionSkin.SpritePath);
        if (!__instance.IsNodeReady()) return;
        
        Texture2D? sprite = LoadTexture(potionSkin.SpritePath);
        if (sprite != null) __instance.Image.Texture = sprite;
        
        Texture2D? outline = LoadTexture(potionSkin.OutlinePath);
        if (outline != null) __instance.Outline.Texture = outline;
    }

    [HarmonyPatch(typeof(PotionModel), "get_Image")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool PotionThrowReplace(PotionModel __instance, ref Texture2D? __result)
    {
        SkinData.PotionSkinData? potionSkin = SkinRegistry.PotionResolve(__instance);
        if (potionSkin == null) return true;
        Texture2D? sprite = LoadTexture(potionSkin.SpritePath);
        if (sprite == null) return true;
        __result = sprite;
        return false;
    }
    
    // Relics
    [HarmonyPatch(typeof(RelicModel), "get_Icon")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool RelicIconReplace(RelicModel __instance, ref Texture2D? __result)
    {
        SkinData.RelicSkinData? relicSkin = SkinRegistry.RelicResolve(__instance);
        if (relicSkin == null) return true;
        Texture2D? sprite = LoadTexture(relicSkin.IconPath);
        if (sprite == null) return true;
        __result = sprite;
        return false;
    }
    
    [HarmonyPatch(typeof(RelicModel), "get_BigIconPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool RelicBigIconReplace(RelicModel __instance, ref string __result)
    {
        SkinData.RelicSkinData? relicSkin = SkinRegistry.RelicResolve(__instance);
        if (relicSkin == null) return true;
        string? iconPath = relicSkin.BigIconPath;
        if (string.IsNullOrWhiteSpace(iconPath)) return true;
        if (!ResourceLoader.Exists(iconPath))
        {
            modEntry.Logger.Warn($"Path {iconPath} is not found!");
            return true;
        }

        __result = iconPath;
        return false;
    }
    
    [HarmonyPatch(typeof(RelicModel), "get_IconOutline")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool RelicOutlineReplace(RelicModel __instance, ref Texture2D? __result)
    {
        SkinData.RelicSkinData? relicSkin = SkinRegistry.RelicResolve(__instance);
        if (relicSkin == null) return true;
        Texture2D? sprite = LoadTexture(relicSkin.OutlinePath);
        if (sprite == null) return true;
        __result = sprite;
        return false;
    }

    [HarmonyPatch(typeof(NRelic), "Reload")]
    [HarmonyPostfix]
    private static void RelicReloadReplace(NRelic __instance)
    {
        if (!__instance.IsNodeReady()) return;

        // Use _model field directly to avoid the "accessed before set" throw
        RelicModel? model = AccessTools.Field(typeof(NRelic), "_model").GetValue(__instance) as RelicModel;
        if (model == null)
        {
            modEntry.Logger.Warn($"Model is null!");
            return;
        }
        modEntry.Logger.Info(model.IconPath);
        SkinData.RelicSkinData? relicSkin = SkinRegistry.RelicResolve(model);
        if (relicSkin == null) return;
        modEntry.Logger.Info(model.IconPath);
        Texture2D? sprite = LoadTexture(relicSkin.IconPath);
        if (sprite != null) __instance.Icon.Texture = sprite;
    }
}

// Orb Darkened Color
[HarmonyPatch]
public static class OrbDarkenedRecolor
{
    static IEnumerable<MethodBase> TargetMethods()
    {
        return AccessTools.AllTypes()
            .Where(t => !t.IsAbstract && typeof(OrbModel).IsAssignableFrom(t))
            .Select(t => t.GetMethod("get_DarkenedColor", BindingFlags.Public | BindingFlags.Instance))
            .Where(m => m != null)
            .Cast<MethodBase>();
    }

    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool Prefix(OrbModel __instance, ref Color __result)
    {
        if (!SkinRegistry.resolveConfig("defect", SkinData.SkinConfigKey.UseDefectOrbs)) return true;
        string orbId = __instance.Id.Entry.ToLower();
        Color? orbColor = SkinRegistry.OrbResolve(orbId).OrbDarkenedColor;
        if (orbColor == null) return true;
        __result = orbColor.Value;
        return false;
    }
}