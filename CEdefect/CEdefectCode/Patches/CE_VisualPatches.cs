using BaseLib.Utils.NodeFactories;
using CEdefect.CEdefectCode.SkinManager;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Relics;

namespace CEdefect.CEdefectCode.Patches;

[HarmonyPatch]
public class CE_VisualPatches
{
    private const string SpriteDefaultPath = "res://CEdefect/scenes/epoque/ce_epoque_combat.tscn";
    private const string SpriteMerchantPath = "res://CEdefect/scenes/epoque/ce_epoque_merchant.tscn";
    private const string SpriteRestPath = "res://CEdefect/scenes/epoque/ce_epoque_rest.tscn";

    [HarmonyPatch(typeof(CharacterModel), "CreateVisuals")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_CombatVisualsPatch(CharacterModel __instance, ref NCreatureVisuals __result)
    {
        CE_Utils.Logger("CombatVisualsPatch has been called.");
        string? path = SkinRegistry.Resolve(p => p.CombatScene);
        if (!CE_Utils.IsDefect(__instance) || !CE_Utils.ResourceAvailable(path)) return true; 
        PackedScene defaultScene = ResourceLoader.Load<PackedScene>(path, (string)null, ResourceLoader.CacheMode.Reuse);
        NCreatureVisuals characterVisual = NodeFactory<NCreatureVisuals>.CreateFromScene(defaultScene);
        __result = characterVisual;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_MerchantAnimPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_MerchantVisualsPatch(CharacterModel __instance, ref string __result)
    {
        if (!CE_Utils.IsDefect(__instance) || !CE_Utils.ResourceAvailable(SpriteMerchantPath)) return true; 
        string? path = SkinRegistry.Resolve(p => p.MerchantScene);
        __result = path;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_RestSiteAnimPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_RestVisualPatch(CharacterModel __instance, ref string __result)
    {
        if (!CE_Utils.IsDefect(__instance) || !CE_Utils.ResourceAvailable(SpriteRestPath)) return true; 
        string? path = SkinRegistry.Resolve(p => p.RestScene);
        __result = path;
        return false;
    }
    
}