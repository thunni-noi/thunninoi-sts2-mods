using System.Reflection;
using BaseLib.Utils;
using BaseLib.Utils.NodeFactories;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using thunninoiSkinManager.thunninoiSkinManagerCode;

namespace MakotoNecrobinder.MakotoNecrobinderCode.Patches;

[HarmonyPatch(typeof(MonsterModel), nameof(MonsterModel.CreateVisuals))]
public class OstyReplace
{
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool Prefix(MonsterModel __instance, ref NCreatureVisuals __result)
    {
        if (!SkinRegistry.IsUsingSkin(ModelDb.Character<Necrobinder>().Id, "seesmakoto")) return true;
        string thanatos_path = "res://MakotoNecrobinder/assets/sees/character_visuals/thanatos.tscn";
        if (__instance is Osty)
        {
            PackedScene? scene = PreloadManager.Cache.GetScene(thanatos_path);
            NCreatureVisuals visuals = NodeFactory<NCreatureVisuals>.CreateFromScene(scene);
            __result = visuals;
            return false;
        }
        return true;
    }
}

[HarmonyPatch(typeof(Osty), nameof(Osty.GenerateAnimator))]
public static class OstyEntry
{
    private static bool Prepare() => Mkt_init.IsBeta(); // run if in beta
    
    [HarmonyPostfix]
    public static void Postfix(MegaSprite controller, ref CreatureAnimator __result)
    {
        if (!SkinRegistry.IsUsingSkin(ModelDb.Character<Necrobinder>().Id, "seesmakoto")) return;
        FieldInfo? currentState = typeof(CreatureAnimator).GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);
        AnimState? currentIdle = currentState.GetValue(__result) as AnimState;
        
        AnimState entryState = new AnimState("entry") { NextState = currentIdle };
        __result.AddAnyState("Entry", entryState);
        currentState.SetValue(__result, entryState);

        var animationState = controller.GetAnimationState();
        animationState.SetAnimation("entry", false, 0);
        animationState.AddAnimation(currentIdle.Id, 0f, true, 0);
    }
}