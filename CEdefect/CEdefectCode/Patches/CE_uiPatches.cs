using CEdefect.CEdefectCode.SkinManager;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Relics;

namespace CEdefect.CEdefectCode.Patches;

[HarmonyPatch]
public class CE_uiPatches
{
    [HarmonyPatch(typeof(CharacterModel), "get_MapMarkerPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_MapMarkerPatch(CharacterModel __instance, ref string __result)
    {
        string? path = SkinRegistry.Resolve(p => p.CharacterMapIcon);
        if (!CE_Utils.IsDefect(__instance) || string.IsNullOrWhiteSpace(path)) return true; 
        __result = path;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_IconTexture")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_CharacterIconPatch(CharacterModel __instance, ref Texture2D __result)
    {
        string? path = SkinRegistry.Resolve(p => p.CharacterIcon);
        if (!CE_Utils.IsDefect(__instance) || string.IsNullOrWhiteSpace(path)) return true; 
        Texture2D charIcon =  ResourceLoader.Load<Texture2D>(path);
        __result = charIcon;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_IconOutlineTexture")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_CharacterIconOutlinePatch(CharacterModel __instance, ref Texture2D __result)
    {
        string? path = SkinRegistry.Resolve(p => p.CharacterIconOutline);
        if (!CE_Utils.IsDefect(__instance) || string.IsNullOrWhiteSpace(path)) return true; 
        Texture2D charIcon =  ResourceLoader.Load<Texture2D>(path);
        __result = charIcon;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_IconPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_CharacterIconScenePatch(CharacterModel __instance, ref string __result)
    {
        string? path = SkinRegistry.Resolve(p => p.CharacterIconScene);
        if (!CE_Utils.IsDefect(__instance) || string.IsNullOrWhiteSpace(path)) return true; 
        __result = path;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_ArmPointingTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_ArmPointing(CharacterModel __instance, ref string __result)
    {
        if (!CE_Config.UseCivilightMultArm) return true;
        if (!CE_Utils.IsDefect(__instance)) return true;
        string? path = SkinRegistry.Resolve(p => p.ArmPointing);
        if (path == null) return true;
        __result = path;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmRockTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_ArmRock(CharacterModel __instance, ref string __result)
    {
        if (!CE_Config.UseCivilightMultArm) return true;
        if (!CE_Utils.IsDefect(__instance)) return true;
        string? path = SkinRegistry.Resolve(p => p.ArmRock);
        if (path == null) return true;
        __result = path;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmPaperTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_ArmPaper(CharacterModel __instance, ref string __result)
    {
        if (!CE_Config.UseCivilightMultArm) return true;
        if (!CE_Utils.IsDefect(__instance)) return true;
        string? path = SkinRegistry.Resolve(p => p.ArmPaper);
        if (path == null) return true;
        __result = path;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmScissorsTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_ArmScissor(CharacterModel __instance, ref string __result)
    {
        if (!CE_Config.UseCivilightMultArm) return true;
        if (!CE_Utils.IsDefect(__instance)) return true;
        string? path = SkinRegistry.Resolve(p => p.ArmScissors);
        if (path == null) return true;
        __result = path;
        return false;
    }
    
}