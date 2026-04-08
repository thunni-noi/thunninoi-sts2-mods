using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace InesSilent.InesSilentCode;

[HarmonyPatch]
public class SilentCardPatches
{
    private const string CardFrameMaterial = "res://InesSilent/assets/ui/cards/frames/card_frame_ines.tres";
    private const string CardTrailVfx = "res://InesSilent/scenes/ui/card_trail_ines.tscn";
    
    [HarmonyPatch(typeof(CardPoolModel), "get_FrameMaterialPath")]
    [HarmonyPrefix]
    private static bool CardFramePatch(CardPoolModel __instance, ref string __result)
    {
        if (!InesConfig.UseInesCardFrame) return true;
        if (!((__instance) is SilentCardPool)) return true;
        __result = CardFrameMaterial;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_TrailPath")]
    [HarmonyPrefix]
    private static bool CardTrailPatch(CharacterModel __instance, ref string __result)
    {
        if (!InesConfig.UseInesCardFrame) return true;
        if (!Utils.IsSilent(__instance) || !Utils.ResourceAvailable(CardTrailVfx)) return true; 
        __result = CardTrailVfx;
        return false;
    }
    
}