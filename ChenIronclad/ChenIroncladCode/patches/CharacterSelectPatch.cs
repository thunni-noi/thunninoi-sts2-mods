using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;

namespace ChenIronclad.ChenIroncladCode.patches;

[HarmonyPatch]
public class CharacterSelectPatch
{
    private const string SceneCharSelBg = "res://ChenIronclad/scenes/ui/chen_charselect_bg.tscn";
    private const string SpriteCharSelPortrait = "res://ChenIronclad/assets/ui/characterSelect/chen_portrait.png";
    private const string CharSelTransition = "res://materials/transitions/silent_transition_mat.tres";
    
   
    
    [HarmonyPatch(typeof(CharacterModel), "get_CharacterSelectBg")]
    [HarmonyPrefix]
    private static bool CharSelectBgPatch(CharacterModel __instance, ref string __result)
    {
        if (!Utils.IsIronclad(__instance)) return true;
        if (!ResourceLoader.Exists(SceneCharSelBg))
        {
            Log.Error($"[CHEN] Cannot find path : {SceneCharSelBg}");
            return true;
        }

        __result = SceneCharSelBg;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_CharacterSelectIconPath")]
    [HarmonyPrefix]
    private static bool CharSelectPortraitPatch(CharacterModel __instance, ref string __result)
    {
        if (!Utils.IsIronclad(__instance)) return true;
        if (!ResourceLoader.Exists(SpriteCharSelPortrait))
        {
            Log.Error($"[CHEN] Cannot find path : {SpriteCharSelPortrait}");
            return true;
        }

        __result = SpriteCharSelPortrait;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_CharacterSelectTransitionPath")]
    [HarmonyPrefix]
    private static bool CharTransitionPatch(CharacterModel __instance, ref string __result)
    {
        if (!Utils.IsIronclad(__instance)) return true;
        if (!ResourceLoader.Exists(CharSelTransition))
        {
            Log.Error($"[CHEN] Cannot find path : {CharSelTransition}");
            return true;
        }

        __result = CharSelTransition;
        return false;
    }
}