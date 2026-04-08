using BaseLib.Utils.NodeFactories;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace InesSilent.InesSilentCode;

[HarmonyPatch]
public class SilentVisualPatches
{
      private const string SpriteDefaultPath = "res://InesSilent/scenes/character/ines_default.tscn";
    private const string SpriteMerchantPath = "res://InesSilent/scenes/character/ines_merchant.tscn";
    private const string SpriteRestPath = "res://InesSilent/scenes/character/ines_rest_site.tscn";

    [HarmonyPatch(typeof(CharacterModel), "CreateVisuals")]
    [HarmonyPrefix]
    private static bool CombatVisualsPatch(CharacterModel __instance, ref NCreatureVisuals __result)
    {
        //Log.Debug($"[CHEN] CombatVisualsPatch has been called.");
        if (!Utils.IsSilent(__instance) || !Utils.ResourceAvailable(SpriteDefaultPath)) return true; 

        PackedScene defaultScene = ResourceLoader.Load<PackedScene>(SpriteDefaultPath, (string)null, ResourceLoader.CacheMode.Reuse);
        NCreatureVisuals characterVisual = NodeFactory<NCreatureVisuals>.CreateFromScene(defaultScene);
        __result = characterVisual;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_MerchantAnimPath")]
    [HarmonyPrefix]
    private static bool MerchantVisualsPatch(CharacterModel __instance, ref string __result)
    {
        if (!Utils.IsSilent(__instance) || !Utils.ResourceAvailable(SpriteMerchantPath)) return true; 
        __result = SpriteMerchantPath;
        return false;
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_RestSiteAnimPath")]
    [HarmonyPrefix]
    private static bool RestVisualPatch(CharacterModel __instance, ref string __result)
    {
        if (!Utils.IsSilent(__instance) || !Utils.ResourceAvailable(SpriteRestPath)) return true; 
        __result = SpriteRestPath;
        return false;
    }

    [HarmonyPatch(typeof(NShivThrowVfx), "ApplyTint")]
    [HarmonyPrefix]
    private static bool NShivThrowVfx_Tint(NShivThrowVfx __instance, ref Color tint)
    {   
        if (InesConfig.RedShivColor) tint = new Color(217f / 255f, 0f, 0f, 1f);
        return true;
    }
}