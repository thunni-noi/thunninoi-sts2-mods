using BaseLib.Utils.NodeFactories;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using Microsoft.VisualBasic.CompilerServices;

namespace InesSilent.InesSilentCode;

[HarmonyPatch]
public class InesVisualPatches
{
    private const string SpriteDefaultPath = "res://InesSilent/scenes/character/ines_default.tscn";
    private const string SpriteMerchantPath = "res://InesSilent/scenes/character/ines_merchant.tscn";
    private const string SpriteRestPath = "res://InesSilent/scenes/character/ines_rest_site.tscn";

    [HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.CreateVisuals))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_CombatVisualsPatch(CharacterModel __instance, ref NCreatureVisuals __result)
    {
        if (!InesUtils.IsSilent(__instance) || !InesUtils.ResourceAvailable(SpriteDefaultPath)) return true; 
        InesUtils.Logger("Patching CharacterModel.CreateVisuals");
        PackedScene defaultScene = ResourceLoader.Load<PackedScene>(SpriteDefaultPath, (string)null, ResourceLoader.CacheMode.Reuse);
        NCreatureVisuals characterVisual = NodeFactory<NCreatureVisuals>.CreateFromScene(defaultScene);
        __result = characterVisual;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_MerchantAnimPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_MerchantVisualsPatch(CharacterModel __instance, ref string __result)
    {
        if (!InesUtils.IsSilent(__instance) || !InesUtils.ResourceAvailable(SpriteMerchantPath)) return true; 
        __result = SpriteMerchantPath;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_RestSiteAnimPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_RestVisualPatch(CharacterModel __instance, ref string __result)
    {
        if (!InesUtils.IsSilent(__instance) || !InesUtils.ResourceAvailable(SpriteRestPath)) return true; 
        __result = SpriteRestPath;
        return false;
    }

    [HarmonyPatch(typeof(NShivThrowVfx), "ApplyTint")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_ShivThrowVfxPatch(NShivThrowVfx __instance, ref Color tint)
    {   
        if (InesConfig.RedShivColor) tint = new Color(217f / 255f, 0f, 0f, 1f);
        return true;
    }
}