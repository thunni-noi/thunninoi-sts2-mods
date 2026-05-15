using System.Reflection;
using BaseLib.Utils.NodeFactories;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

[HarmonyPatch]
public class CharacterVisualPatches
{
    [HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.CreateVisuals))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool combatVisualsReplace(CharacterModel __instance, ref NCreatureVisuals __result)
    {
        string charId = __instance.Id.Entry.ToLower();
        string? skinVisualPath = SkinRegistry.PathResolve(charId, skin => skin.CombatVisuals);
        if (string.IsNullOrWhiteSpace(skinVisualPath)) return true;

        PackedScene? scene = ResourceLoader.Load<PackedScene>(skinVisualPath, null, ResourceLoader.CacheMode.Reuse);
        NCreatureVisuals visuals = NodeFactory<NCreatureVisuals>.CreateFromScene(scene);
        __result = visuals;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_MerchantAnimPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool merchanVisualReplace(CharacterModel __instance, ref string __result)
    {
        string charId = __instance.Id.Entry.ToLower();
        string? skinPath = SkinRegistry.PathResolve(charId, skin => skin.MerchantAnim);
        if (string.IsNullOrWhiteSpace(skinPath)) return true;
        __result = skinPath;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_RestSiteAnimPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool restVisualReplace(CharacterModel __instance, ref string __result)
    {
        string charId = __instance.Id.Entry.ToLower();
        string? skinPath = SkinRegistry.PathResolve(charId, skin => skin.RestSiteAnim);
        if (string.IsNullOrWhiteSpace(skinPath)) return true;
        __result = skinPath;
        return false;
    }

    private static bool ArmReplace(CharacterModel __instance, Func<SkinData, string?> selector, ref string __result)
    {
        string charId = __instance.Id.Entry.ToLower();
        string armPath = SkinRegistry.PathResolve(charId, selector);
        if (string.IsNullOrWhiteSpace(armPath)) return true;
        __result = armPath;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_ArmPointingTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool ArmPointReplace(CharacterModel __instance, ref string __result) =>
        ArmReplace(__instance, s => s.HandPoint, ref __result);
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmRockTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool ArmRockReplace(CharacterModel __instance, ref string __result) =>
        ArmReplace(__instance, s => s.HandRock, ref __result);
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmPaperTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool ArmPaperReplace(CharacterModel __instance, ref string __result) =>
        ArmReplace(__instance, s => s.HandPaper, ref __result);
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmScissorsTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool ArmScissorsReplace(CharacterModel __instance, ref string __result) =>
        ArmReplace(__instance, s => s.HandScissors, ref __result);
    
    [HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.GenerateAnimator))]
    [HarmonyPostfix]
    public static void AddEntryAnimationPostfix(
        CharacterModel __instance,
        MegaSprite controller,
        ref CreatureAnimator __result)
    {
        if (!controller.HasAnimation("entry")) return;
        
        FieldInfo? currentState = typeof(CreatureAnimator)
            .GetField("_currentState",
                BindingFlags.NonPublic | BindingFlags.Instance);

        AnimState? baseIdle = currentState?.GetValue(__result) as AnimState;
        if (baseIdle == null) return;
        
        AnimState entryState = new AnimState("entry")
        {
            NextState = baseIdle 
        };
        
        __result.AddAnyState("Entry", entryState);

        currentState?.SetValue(__result, entryState);

        var animationState = controller.GetAnimationState();
        animationState.SetAnimation("entry", false, 0);
        animationState.AddAnimation(baseIdle.Id, 0f, true, 0);
    }
}


