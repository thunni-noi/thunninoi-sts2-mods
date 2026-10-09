using System.Runtime.CompilerServices;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using thunninoiSkinManager.thunninoiSkinManagerCode;

namespace MakotoNecrobinder.MakotoNecrobinderCode.Patches;

[HarmonyPatch(typeof(CreatureCmd), nameof(CreatureCmd.TriggerAnim))]
public class EvokerSfx
{
    [HarmonyPostfix]
    private static void Postfix(Creature creature, string triggerName)
    {
        if (!creature.IsPlayer || creature.IsDead) return;
        if (creature?.Player?.Character is not Necrobinder) return;
        if (!SkinRegistry.IsUsingSkin(ModelDb.Character<Necrobinder>().Id, "seesmakoto")) return;
        
        if (triggerName == "summonTrigger") castAnim(creature, 0);
        else if (triggerName == "Cast") castAnim(creature, 0.5);
    }

    static void castAnim(Creature target, double delays)
    {
        if (delays <= 0)
        {
            playAnim(target);
            return;
        }
        
        if (Engine.GetMainLoop() is SceneTree tree)
            tree.CreateTimer(delays).Timeout += () => playAnim(target);
    }

    static void playAnim(Creature target)
    {
        MktAudio.Sound.Play(volumeMult: (float) MktConfig.EvokerSfxVolume / 100);
        VfxHelper.PlayPersonaVfx(target);
    }
}


