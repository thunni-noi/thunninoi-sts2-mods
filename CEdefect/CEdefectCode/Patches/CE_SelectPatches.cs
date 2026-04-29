using CEdefect.CEdefectCode.SkinManager;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace CEdefect.CEdefectCode.Patches;

[HarmonyPatch]
public class CE_SelectPatches
{
    //private const string SceneCharSelBg = "res://CEdefect/scenes/epoque/charselect/ce_epoque_charsel.tscn";
    //private const string SpriteCharSelPortrait = "res://InesSilent/assets/ui/characterSelect/ines_portrait.png";
    
    [HarmonyPatch(typeof(CharacterModel), "get_CharacterSelectBg")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CharSelectBgPatch(CharacterModel __instance, ref string __result)
    {
        string? path = SkinRegistry.Resolve(p => p.CharacterSelectBg);
        if (!CE_Utils.IsDefect(__instance) || string.IsNullOrWhiteSpace(path)) return true; 
        __result = path;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_CharacterSelectIconPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CharSelectPortraitPatch(CharacterModel __instance, ref string __result)
    {
        string? path = SkinRegistry.Resolve(p => p.CharacterSelectPortrait);
        if (!CE_Utils.IsDefect(__instance) || string.IsNullOrWhiteSpace(path)) return true; 
        __result = path;
        return false;
    }

}