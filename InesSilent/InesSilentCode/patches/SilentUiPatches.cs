using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Potions;

namespace InesSilent.InesSilentCode;

[HarmonyPatch]
public class SilentUiPatches
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
    private static bool MapMarkerPatch(CharacterModel __instance, ref string __result)
    {
        if (!Utils.IsSilent(__instance) || !Utils.ResourceAvailable(CharacterMapMarker)) return true; 
        __result = CharacterMapMarker;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_IconTexture")]
    [HarmonyPrefix]
    private static bool CharacterIconPatch(CharacterModel __instance, ref Texture2D __result)
    {
        if (!Utils.IsSilent(__instance) || !Utils.ResourceAvailable(CharacterIcon)) return true; 
        Texture2D charIcon =  ResourceLoader.Load<Texture2D>(CharacterIcon);
        __result = charIcon;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_IconOutlineTexture")]
    [HarmonyPrefix]
    private static bool CharacterIconOutlinePatch(CharacterModel __instance, ref Texture2D __result)
    {
        if (!Utils.IsSilent(__instance) || !Utils.ResourceAvailable(CharacterIconOutline)) return true; 
        Texture2D charIcon =  ResourceLoader.Load<Texture2D>(CharacterIconOutline);
        __result = charIcon;
        return false;
    }
    
    // sometimes singleplayer won't load patched texture, this is for safeguard i guess?
    [HarmonyPatch(typeof(CharacterModel), "get_IconPath")]
    [HarmonyPrefix]
    private static bool CharacterIconScenePatch(CharacterModel __instance, ref string __result)
    {
        if (!Utils.IsSilent(__instance) || !Utils.ResourceAvailable(SceneCharacterIcon)) return true; 
        __result = SceneCharacterIcon;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmPointingTexturePath")]
    [HarmonyPrefix]
    private static bool ArmRockPatch(CharacterModel __instance, ref string __result)
    {
        if (!InesConfig.UseInesMultArm) return true;
        if (!Utils.IsSilent(__instance) || !Utils.ResourceAvailable(CharacterArmPoint)) return true; 
        __result = CharacterArmPoint;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmRockTexturePath")]
    [HarmonyPrefix]
    private static bool ArmPaperPatch(CharacterModel __instance, ref string __result)
    {
        if (!InesConfig.UseInesMultArm) return true;
        if (!Utils.IsSilent(__instance) || !Utils.ResourceAvailable(CharacterArmRock)) return true; 
        __result = CharacterArmRock;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmPaperTexturePath")]
    [HarmonyPrefix]
    private static bool ArmScissorPatch(CharacterModel __instance, ref string __result)
    {
        if (!InesConfig.UseInesMultArm) return true;
        if (!Utils.IsSilent(__instance) || !Utils.ResourceAvailable(CharacterArmScissor)) return true; 
        __result = CharacterArmPaper;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmScissorsTexturePath")]
    [HarmonyPrefix]
    private static bool ArmPointingPatch(CharacterModel __instance, ref string __result)
    {
        if (!InesConfig.UseInesMultArm) return true;
        if (!Utils.IsSilent(__instance) || !Utils.ResourceAvailable(CharacterArmPoint)) return true; 
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
    private static bool WisadelePowerBigPatch(PowerModel __instance, ref Texture2D __result)
    {
        if (!InesConfig.UseWisadelePotion) return true;
        if (!(__instance is PoisonPower)) return true;
        Texture2D texture = ResourceLoader.Load<Texture2D>(WisadelePowerTexture, null, ResourceLoader.CacheMode.Reuse);
        __result = texture;
        return false;
    }
}