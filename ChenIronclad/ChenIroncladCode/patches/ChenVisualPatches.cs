using BaseLib.Utils.NodeFactories;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;

namespace ChenIronclad.ChenIroncladCode.patches;

[HarmonyPatch]
public class ChenVisualPatches
{
    private const string SpriteDefaultPath = "res://ChenIronclad/scenes/character/chen_dawnstreak.tscn";
    private const string SpriteMerchantPath = "res://ChenIronclad/scenes/character/chen_dawnstreak_merchant.tscn";
    private const string SpriteRestPath = "res://ChenIronclad/scenes/character/chen_dawnstreak_rest_site.tscn";
    private const string HellraiserScene = "res://ChenIronclad/scenes/misc/chen_hellraiser_sword_vfx.tscn";

    [HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.CreateVisuals))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Chen_CombatVisualsPatch(CharacterModel __instance, ref NCreatureVisuals __result)
    {
        if (ChenConfig.DebugMode) Log.Info($"[CHEN] Patching CharacterModel.CreateVisuals from {__instance.Id}");
        if (!ChenUtils.IsIronclad(__instance)) return true;

        if (!ResourceLoader.Exists(SpriteDefaultPath))
        {
            Log.Error($"[CHEN] Cannot find path : {SpriteDefaultPath}");
            return true;
        }

        var defaultScene = ResourceLoader.Load<PackedScene>(SpriteDefaultPath, (string)null, ResourceLoader.CacheMode.Reuse);
        var characterVisual = NodeFactory<NCreatureVisuals>.CreateFromScene(defaultScene);
        __result = characterVisual;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_MerchantAnimPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Chen_MerchantVisualsPatch(CharacterModel __instance, ref string __result)
    {
        if (ChenConfig.DebugMode) Log.Info($"[CHEN] Patching CharacterModel.MerchantAnimPath from {__instance.Id}");
        if (!ChenUtils.IsIronclad(__instance)) return true;

        if (!ResourceLoader.Exists(SpriteMerchantPath))
        {
            Log.Error($"[CHEN] Cannot find path : {SpriteMerchantPath}");
            return true;
        }
        __result = SpriteMerchantPath;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_RestSiteAnimPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Chen_RestVisualPatch(CharacterModel __instance, ref string __result)
    {
        if (ChenConfig.DebugMode) Log.Info($"[CHEN] Patching CharacterModel.RestSiteAnimPath from {__instance.Id}");
        if (!ChenUtils.IsIronclad(__instance)) return true;

        if (!ResourceLoader.Exists(SpriteRestPath))
        {
            Log.Error($"[CHEN] Cannot find path : {SpriteRestPath}");
            return true;
        }
        __result = SpriteRestPath;
        return false;
    }
    
}