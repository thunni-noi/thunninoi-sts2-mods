using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace ChenIronclad.ChenIroncladCode.patches;

[HarmonyPatch]
public class ChenCardPatches
{

    private const string CardFrameMaterial = "res://ChenIronclad/assets/ui/cards/frames/card_frame_chen.tres";
    private const string CardTrailVfx = "res://ChenIronclad/scenes/ui/chen_card_trail.tscn";
    
    [HarmonyPatch(typeof(CardPoolModel), "get_FrameMaterialPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Chen_CardFramePatch(CardPoolModel __instance, ref string __result)
    {
        if (ChenConfig.DebugMode) Log.Info($"[CHEN] Patching CardPoolModel.FrameMaterialPath from {__instance.Id}");
        if (!ChenConfig.UseChenCardFrame) return true;
        if (!((__instance) is IroncladCardPool)) return true;
        __result = CardFrameMaterial;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_TrailPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Chen_CardTrailPatch(CharacterModel __instance, ref string __result)
    {
        if (ChenConfig.DebugMode) Log.Info($"[CHEN] Patching CharacterModel.TrailPath from {__instance.Id}");
        if (!ChenConfig.UseChenCardFrame) return true;
        if (!ChenUtils.IsIronclad(__instance)) return true;
        if (!ResourceLoader.Exists(CardTrailVfx))
        {
            Log.Error($"[CHEN] Cannot find path : {CardTrailVfx}");
            return false;
        }
        __result = CardTrailVfx;
        return false;
    }
}