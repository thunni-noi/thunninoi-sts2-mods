using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace InesSilent.InesSilentCode;

[HarmonyPatch]
public class InesCardPatches
{
    private const string CardFrameMaterial = "res://InesSilent/assets/ui/cards/frames/card_frame_ines.tres";
    private const string CardTrailVfx = "res://InesSilent/scenes/ui/card_trail_ines.tscn";
    
    [HarmonyPatch(typeof(CardPoolModel), "get_FrameMaterialPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_CardFramePatch(CardPoolModel __instance, ref string __result)
    {
        if (!InesConfig.UseInesCardFrame) return true;
        if (!((__instance) is SilentCardPool)) return true;
        __result = CardFrameMaterial;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_TrailPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_CardTrailPatch(CharacterModel __instance, ref string __result)
    {
        if (!InesConfig.UseInesCardFrame) return true;
        if (!InesUtils.IsSilent(__instance) || !InesUtils.ResourceAvailable(CardTrailVfx)) return true; 
        __result = CardTrailVfx;
        return false;
    }
    
}