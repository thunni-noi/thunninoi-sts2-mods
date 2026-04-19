using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Potions;

namespace InesSilent.InesSilentCode;

[HarmonyPatch]
public class InesUiPatches
{
     private const string CharacterIcon = "res://InesSilent/assets/ui/icon/character_icon_ines.png";
    private const string CharacterIconOutline = "res://InesSilent/assets/ui/icon/character_icon_outline_ines.png";
    private const string CharacterMapMarker = "res://InesSilent/assets/ui/icon/map_marker_ines.png";
    private const string SceneCharacterIcon = "res://InesSilent/scenes/ui/ines_icon.tscn";

    private const string CharacterArmPoint = "res://InesSilent/assets/ui/arm/ines_point.png";
    private const string CharacterArmRock = "res://InesSilent/assets/ui/arm/ines_rock.png";
    private const string CharacterArmPaper = "res://InesSilent/assets/ui/arm/ines_paper.png";
    private const string CharacterArmScissor = "res://InesSilent/assets/ui/arm/ines_scissor.png";

    private const string WisadeleTexture = "res://InesSilent/assets/ui/icon/wisadele/wisadele_potion.png";
    private const string WisadeleOutlineTexture = "res://InesSilent/assets/ui/icon/wisadele/wisadele_potion_outline.png";
    private const string WisadelePowerTexture = "res://InesSilent/assets/ui/icon/wisadele/wisadele_power.png";

    [HarmonyPatch(typeof(CharacterModel), "get_MapMarkerPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_MapMarkerPatch(CharacterModel __instance, ref string __result)
    {
        if (!InesUtils.IsSilent(__instance) || !InesUtils.ResourceAvailable(CharacterMapMarker)) return true; 
        __result = CharacterMapMarker;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_IconTexture")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_CharacterIconPatch(CharacterModel __instance, ref Texture2D __result)
    {
        if (!InesUtils.IsSilent(__instance) || !InesUtils.ResourceAvailable(CharacterIcon)) return true; 
        Texture2D charIcon =  ResourceLoader.Load<Texture2D>(CharacterIcon);
        __result = charIcon;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_IconOutlineTexture")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_CharacterIconOutlinePatch(CharacterModel __instance, ref Texture2D __result)
    {
        if (!InesUtils.IsSilent(__instance) || !InesUtils.ResourceAvailable(CharacterIconOutline)) return true; 
        Texture2D charIcon =  ResourceLoader.Load<Texture2D>(CharacterIconOutline);
        __result = charIcon;
        return false;
    }
    
    // singleplay top icon
    [HarmonyPatch(typeof(CharacterModel), "get_IconPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_CharacterIconScenePatch(CharacterModel __instance, ref string __result)
    {
        if (!InesUtils.IsSilent(__instance) || !InesUtils.ResourceAvailable(SceneCharacterIcon)) return true; 
        __result = SceneCharacterIcon;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmPointingTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_ArmPointing(CharacterModel __instance, ref string __result)
    {
        if (!InesConfig.UseInesMultArm) return true;
        if (!InesUtils.IsSilent(__instance) || !InesUtils.ResourceAvailable(CharacterArmPoint)) return true; 
        __result = CharacterArmPoint;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmRockTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_ArmRock(CharacterModel __instance, ref string __result)
    {
        if (!InesConfig.UseInesMultArm) return true;
        if (!InesUtils.IsSilent(__instance) || !InesUtils.ResourceAvailable(CharacterArmRock)) return true; 
        __result = CharacterArmRock;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmPaperTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_ArmPaper(CharacterModel __instance, ref string __result)
    {
        if (!InesConfig.UseInesMultArm) return true;
        if (!InesUtils.IsSilent(__instance) || !InesUtils.ResourceAvailable(CharacterArmPaper)) return true; 
        __result = CharacterArmPaper;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmScissorsTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_ArmScissor(CharacterModel __instance, ref string __result)
    {
        if (!InesConfig.UseInesMultArm) return true;
        if (!InesUtils.IsSilent(__instance) || !InesUtils.ResourceAvailable(CharacterArmScissor)) return true; 
        __result = CharacterArmScissor;
        return false;
    }

    [HarmonyPatch(typeof(NPotion), "Reload")]
    [HarmonyPostfix]
    private static void WisadlePotionPatch(NPotion __instance)
    {
        if (!InesConfig.UseWisadelePotion) return;
        if (__instance.Model is not PoisonPotion) return;
        if (!__instance.IsNodeReady()) return;
        Texture2D texture = ResourceLoader.Load<Texture2D>(WisadeleTexture, null, ResourceLoader.CacheMode.Reuse);
        Texture2D textureOutline = ResourceLoader.Load<Texture2D>(WisadeleOutlineTexture, null, ResourceLoader.CacheMode.Reuse);
        __instance.Image.Texture = texture;
        __instance.Outline.Texture = textureOutline;
    }
    
    [HarmonyPatch(typeof(PotionModel), "get_Image")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool WisadlePotionThrownPatch(PotionModel __instance, ref Texture2D __result)
    {
        if (!InesConfig.UseWisadelePotion) return true;
        if (!(__instance is PoisonPotion)) return true;
        Texture2D texture = ResourceLoader.Load<Texture2D>(WisadeleTexture, null, ResourceLoader.CacheMode.Reuse);
        __result = texture;
        return false;
    }
    
    [HarmonyPatch(typeof(PowerModel), "get_Icon")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool WisadelePowerPatch(PowerModel __instance, ref Texture2D __result)
    {
        if (!InesConfig.UseWisadelePotion) return true;
        if (!(__instance is PoisonPower)) return true;
        Texture2D texture = ResourceLoader.Load<Texture2D>(WisadelePowerTexture, null, ResourceLoader.CacheMode.Reuse);
        __result = texture;
        return false;
    }
    
    [HarmonyPatch(typeof(PowerModel), "get_BigIcon")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool WisadelePowerBigPatch(PowerModel __instance, ref Texture2D __result)
    {
        if (!InesConfig.UseWisadelePotion) return true;
        if (!(__instance is PoisonPower)) return true;
        Texture2D texture = ResourceLoader.Load<Texture2D>(WisadelePowerTexture, null, ResourceLoader.CacheMode.Reuse);
        __result = texture;
        return false;
    }
}