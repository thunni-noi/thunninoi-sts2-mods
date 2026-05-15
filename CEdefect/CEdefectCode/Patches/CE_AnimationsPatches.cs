using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;

namespace CEdefect.CEdefectCode.Patches;

[HarmonyPatch]
public static class CE_AnimationsPatches
{
    // (Note for my future me)
    // Below is a garbled up code that is the result of many many many duct tape put together to achieve very minor things people would not noticed
    // If this break (which is very likely), either just don't bother to fix it or spent unreasonable amount of time to fix it, your call.
    
    private static readonly HashSet<Creature> _castLoopActive = new();
    private static readonly HashSet<object> _skipReentry = new();

    // Function to continue looping the animation if cards played is somewhat long (Voltaic, multicast etc)
    private static async Task TriggerCastLoop(CardModel card, Func<Task> onPlay)
    {
        Creature creature = card.Owner.Creature;
        float delay = card.Owner.Character.CastAnimDelay;
        _castLoopActive.Add(creature);
        try
        {
            await CreatureCmd.TriggerAnim(creature, "CastStart", delay);
            //await CreatureCmd.TriggerAnim(creature, "CastLoop", delay * 2);
            await onPlay();
            await CreatureCmd.TriggerAnim(creature, "CastEnd", delay);
        }
        finally
        {
            _castLoopActive.Remove(creature);
        }
    }

    [HarmonyPatch(typeof(CreatureCmd), nameof(CreatureCmd.TriggerAnim))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool SuppressGameAnim(Creature creature, string triggerName, ref Task __result)
    {
        if (!CE_Utils.isUsingSkin()) return true;
        if (CE_Config.UseCeCompatMode) return true;
        if (!string.Equals(triggerName, "Cast", StringComparison.Ordinal)) return true;
        //if (!string.Equals(triggerName, "Attack", StringComparison.Ordinal)) return true;
        if (!_castLoopActive.Contains(creature)) return true;
        __result = Task.CompletedTask;
        return false;
    }
    

    [HarmonyPatch(typeof(Voltaic), "OnPlay")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool VoltaicPatch(Voltaic __instance, PlayerChoiceContext choiceContext,
        CardPlay cardPlay, ref Task __result)
    {
        if (!CE_Utils.isUsingSkin()) return true;
        if (CE_Config.UseCeCompatMode) return true;
        if (__instance.Owner.Character is not Defect) return true;
        // prevent infinite recursion
        if (_skipReentry.Contains(__instance)) return true;
 
        int count = (int)((CalculatedVar)__instance.DynamicVars["CalculatedChannels"]).Calculate(cardPlay.Target);
        if (count <= 3) return true;
 
        __result = TriggerCastLoop(__instance, async () =>
        {
            _skipReentry.Add(__instance);
            try
            {
               // call base game's logic
                MethodInfo onPlay = AccessTools.Method(typeof(Voltaic), "OnPlay");
                await (Task)onPlay.Invoke(__instance, new object[] { choiceContext, cardPlay });
            }
            finally
            {
                _skipReentry.Remove(__instance);
            }
        });
 
        return false;
    }
    
    [HarmonyPatch(typeof(MultiCast), "OnPlay")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool MulticastPatch(MultiCast __instance, PlayerChoiceContext choiceContext,
        CardPlay cardPlay, ref Task __result)
    {
        if (!CE_Utils.isUsingSkin()) return true;
        if (CE_Config.UseCeCompatMode) return true;
        if (__instance.Owner.Character is not Defect) return true;
        if (_skipReentry.Contains(__instance)) return true;
 
        int evokeCount = __instance.ResolveEnergyXValue();
        if (__instance.IsUpgraded) evokeCount++;
        if (evokeCount <= 4) return true;
      
 
        __result = TriggerCastLoop(__instance, async () =>
        {
            _skipReentry.Add(__instance);
            try
            {
                MethodInfo onPlay = AccessTools.Method(typeof(MultiCast), "OnPlay");
                await (Task)onPlay.Invoke(__instance, new object[] { choiceContext, cardPlay });
            }
            finally
            {
                _skipReentry.Remove(__instance);
            }
        });
 
        return false;
    }
    
    [HarmonyPatch(typeof(Tempest), "OnPlay")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool TempestPatch(MultiCast __instance, PlayerChoiceContext choiceContext,
        CardPlay cardPlay, ref Task __result)
    {
        if (!CE_Utils.isUsingSkin()) return true;
        if (CE_Config.UseCeCompatMode) return true;
        if (__instance.Owner.Character is not Defect) return true;
        if (_skipReentry.Contains(__instance)) return true;
 
        int orbCount = __instance.ResolveEnergyXValue();
        if (__instance.IsUpgraded) orbCount++;
        if (orbCount <= 4) return true;
      
 
        __result = TriggerCastLoop(__instance, async () =>
        {
            _skipReentry.Add(__instance);
            try
            {
                MethodInfo onPlay = AccessTools.Method(typeof(Tempest), "OnPlay");
                await (Task)onPlay.Invoke(__instance, new object[] { choiceContext, cardPlay });
            }
            finally
            {
                _skipReentry.Remove(__instance);
            }
        });
 
        return false;
    }
    
    
}