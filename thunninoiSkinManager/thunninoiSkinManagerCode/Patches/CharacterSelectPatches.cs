using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

[HarmonyPatch]
public class CharacterSelectPatches
{

    [HarmonyPatch(typeof(CharacterModel), "get_CharacterSelectBg")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CharacterSelectReplace(CharacterModel __instance, ref string __result)
    {
        string charId = __instance.Id.Entry.ToLower();
        
        string? skinBgPath = SkinRegistry.PathResolve(charId, skin => skin.CharacterSelectBg);
        if (string.IsNullOrWhiteSpace(skinBgPath)) return true;
        __result = skinBgPath;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_CharacterSelectIconPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CharacterSelectPortraitReplace(CharacterModel __instance, ref string __result)
    {
        string charId = __instance.Id.Entry.ToLower();
        
        string? skinPortraitPath = SkinRegistry.PathResolve(charId, skin => skin.CharacterSelectIcon);
        if (string.IsNullOrWhiteSpace(skinPortraitPath)) return true;
        __result = skinPortraitPath;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_CharacterSelectTransitionPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CharacterSelectTransitionReplace(CharacterModel __instance, ref string __result)
    {
        string charId = __instance.Id.Entry.ToLower();
        string? skinPath = SkinRegistry.PathResolve(charId, skin => skin.CharacterSelectTransition);
        if (string.IsNullOrWhiteSpace(skinPath)) return true;
        __result = skinPath;
        return false;
    }
}