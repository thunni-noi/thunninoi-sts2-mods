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
public class CharacterVisuals
{
    private const string SpriteDefaultPath = "res://ChenIronclad/scenes/character/chen_dawnstreak.tscn";
    private const string SpriteMerchantPath = "res://ChenIronclad/scenes/character/chen_dawnstreak_merchant.tscn";
    private const string SpriteRestPath = "res://ChenIronclad/scenes/character/chen_dawnstreak_rest_site.tscn";
    private const string HellraiserScene = "res://ChenIronclad/scenes/misc/chen_hellraiser_sword_vfx.tscn";

    [HarmonyPatch(typeof(CharacterModel), "CreateVisuals")]
    [HarmonyPrefix]
    private static bool CombatVisualsPatch(CharacterModel __instance, ref NCreatureVisuals __result)
    {
        //Log.Debug($"[CHEN] CombatVisualsPatch has been called.");
        if (!Utils.IsIronclad(__instance)) return true;

        if (!ResourceLoader.Exists(SpriteDefaultPath))
        {
            Log.Error($"[CHEN] Cannot find path : {SpriteDefaultPath}");
            return true;
        }

        PackedScene defaultScene = ResourceLoader.Load<PackedScene>(SpriteDefaultPath, (string)null, ResourceLoader.CacheMode.Reuse);
        NCreatureVisuals characterVisual = NodeFactory<NCreatureVisuals>.CreateFromScene(defaultScene);
        __result = characterVisual;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_MerchantAnimPath")]
    [HarmonyPrefix]
    private static bool MerchantVisualsPatch(CharacterModel __instance, ref string __result)
    {
        if (!Utils.IsIronclad(__instance)) return true;

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
    private static bool RestVisualPatch(CharacterModel __instance, ref string __result)
    {
        if (!Utils.IsIronclad(__instance)) return true;

        if (!ResourceLoader.Exists(SpriteRestPath))
        {
            Log.Error($"[CHEN] Cannot find path : {SpriteRestPath}");
            return true;
        }
        __result = SpriteRestPath;
        return false;
    }
    
}