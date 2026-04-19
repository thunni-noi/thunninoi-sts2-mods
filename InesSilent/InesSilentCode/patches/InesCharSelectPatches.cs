using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace InesSilent.InesSilentCode;

[HarmonyPatch]
public class InesCharSelectPatches
{
    private const string SceneCharSelBg = "res://InesSilent/scenes/ui/char_select_bg_ines.tscn";
    private const string SpriteCharSelPortrait = "res://InesSilent/assets/ui/characterSelect/ines_portrait.png";
    
    [HarmonyPatch(typeof(CharacterModel), "get_CharacterSelectBg")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool CharSelectBgPatch(CharacterModel __instance, ref string __result)
    {
        if (!InesUtils.IsSilent(__instance) || !InesUtils.ResourceAvailable(SceneCharSelBg)) return true; 
        InesUtils.Logger("Replacing CharSelectBg");
        __result = SceneCharSelBg;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_CharacterSelectIconPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool CharSelectPortraitPatch(CharacterModel __instance, ref string __result)
    {
        if (!InesUtils.IsSilent(__instance) || !InesUtils.ResourceAvailable(SpriteCharSelPortrait)) return true; 
        InesUtils.Logger("Replacing CharSelectPortrait");
        __result = SpriteCharSelPortrait;
        return false;
    }

}