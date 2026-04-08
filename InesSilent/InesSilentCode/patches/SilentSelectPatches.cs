using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;

namespace InesSilent.InesSilentCode;

[HarmonyPatch]
public class SilentSelectPatches
{
    private const string SceneCharSelBg = "res://InesSilent/scenes/ui/char_select_bg_ines.tscn";
    private const string SpriteCharSelPortrait = "res://InesSilent/assets/ui/characterSelect/ines_portrait.png";
    
    [HarmonyPatch(typeof(CharacterModel), "get_CharacterSelectBg")]
    [HarmonyPrefix]
    private static bool CharSelectBgPatch(CharacterModel __instance, ref string __result)
    {
        if (!Utils.IsSilent(__instance) || !Utils.ResourceAvailable(SceneCharSelBg)) return true; 
        __result = SceneCharSelBg;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_CharacterSelectIconPath")]
    [HarmonyPrefix]
    private static bool CharSelectPortraitPatch(CharacterModel __instance, ref string __result)
    {
        if (!Utils.IsSilent(__instance) || !Utils.ResourceAvailable(SpriteCharSelPortrait)) return true; 
        __result = SpriteCharSelPortrait;
        return false;
    }

}