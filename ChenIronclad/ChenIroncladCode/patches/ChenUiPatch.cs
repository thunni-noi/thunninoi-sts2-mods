using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;

namespace ChenIronclad.ChenIroncladCode.patches;

[HarmonyPatch]
public class ChenUiPatch
{
    private const string CharacterIcon = "res://ChenIronclad/assets/ui/icon/character_icon_chen.png";
    private const string CharacterIconOutline = "res://ChenIronclad/assets/ui/icon/character_icon_outline_chen.png";
    private const string CharacterMapMarker = "res://ChenIronclad/assets/ui/icon/map_icon_chen.png";
    private const string SceneCharacterIcon = "res://ChenIronclad/scenes/ui/chen_icon.tscn";

    private const string CharacterArmPoint = "res://ChenIronclad/assets/ui/arm/chen_pointing.png";
    private const string CharacterArmRock = "res://ChenIronclad/assets/ui/arm/chen_rock.png";
    private const string CharacterArmPaper = "res://ChenIronclad/assets/ui/arm/chen_paper.png";
    private const string CharacterArmScissor = "res://ChenIronclad/assets/ui/arm/chen_scissor.png";

    [HarmonyPatch(typeof(CharacterModel), "get_MapMarkerPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Chen_MapMarkerPatch(CharacterModel __instance, ref string __result)
    {
        if (!ChenUtils.IsIronclad(__instance)) return true;
        if (!ResourceLoader.Exists(CharacterMapMarker))
        {
            Log.Error($"[CHEN] Cannot find resource : {CharacterMapMarker}");
            return true;
        }
        __result = CharacterMapMarker;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_IconTexture")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Chen_CharacterIconPatch(CharacterModel __instance, ref Texture2D __result)
    {
        if (!ChenUtils.IsIronclad(__instance)) return true;
        if (!ResourceLoader.Exists(CharacterIcon))
        {
            Log.Error($"[CHEN] Cannot find resource : {CharacterIcon}");
            return true;
        }
        Texture2D charIcon =  ResourceLoader.Load<Texture2D>(CharacterIcon);
        __result = charIcon;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_IconOutlineTexture")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Chen_CharacterIconOutlinePatch(CharacterModel __instance, ref Texture2D __result)
    {
        if (!ChenUtils.IsIronclad(__instance)) return true;
        if (!ResourceLoader.Exists(CharacterIconOutline))
        {
            Log.Error($"[CHEN] Cannot find resource : {CharacterIconOutline}");
            return true;
        }
        Texture2D charIcon =  ResourceLoader.Load<Texture2D>(CharacterIconOutline);
        __result = charIcon;
        return false;
    }
    
    // sometimes singleplayer won't load patched texture, this is for safeguard i guess?
    [HarmonyPatch(typeof(CharacterModel), "get_IconPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Chen_CharacterIconScenePatch(CharacterModel __instance, ref string __result)
    {
        if (!ChenUtils.IsIronclad(__instance)) return true;
        if (!ResourceLoader.Exists(SceneCharacterIcon))
        {
            Log.Error($"[CHEN] Cannot find resource : {SceneCharacterIcon}");
            return true;
        }
        __result = SceneCharacterIcon;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmPointingTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Chen_ArmRockPatch(CharacterModel __instance, ref string __result)
    {
        if (!ChenConfig.UseChenMultArm) return true;
        if (!ChenUtils.IsIronclad(__instance)) return true;
        if (!ResourceLoader.Exists(CharacterArmPoint)) return true;
        __result = CharacterArmPoint;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmRockTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Chen_ArmPaperPatch(CharacterModel __instance, ref string __result)
    {
        if (!ChenConfig.UseChenMultArm) return true;
        if (!ChenUtils.IsIronclad(__instance)) return true;
        if (!ResourceLoader.Exists(CharacterArmRock)) return true;
        __result = CharacterArmRock;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmPaperTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Chen_ArmScissorPatch(CharacterModel __instance, ref string __result)
    {
        if (!ChenConfig.UseChenMultArm) return true;
        if (!ChenUtils.IsIronclad(__instance)) return true;
        if (!ResourceLoader.Exists(CharacterArmPaper)) return true;
        __result = CharacterArmPaper;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmScissorsTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Chen_ArmPointingPatch(CharacterModel __instance, ref string __result)
    {
        if (!ChenConfig.UseChenMultArm) return true;
        if (!ChenUtils.IsIronclad(__instance)) return true;
        if (!ResourceLoader.Exists(CharacterArmScissor)) return true;
        __result = CharacterArmScissor;
        return false;
    }
    
} 