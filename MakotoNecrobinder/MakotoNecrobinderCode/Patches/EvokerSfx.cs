using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using thunninoiSkinManager.thunninoiSkinManagerCode;

namespace MakotoNecrobinder.MakotoNecrobinderCode.Patches;

[HarmonyPatch]
public class EvokerSfx
{
    private static CreatureAnimator? _necrobinderAnimator;
    private static Creature? _necrobinderCreature;
    
    internal static void TagAnimator(CharacterModel model, CreatureAnimator animator, Creature creature)
    {
        
        if (model is not Necrobinder) return;
        _necrobinderAnimator = animator;
        _necrobinderCreature = creature;
    }
    [HarmonyPatch(typeof(CreatureAnimator), "SetNextState")]
    [HarmonyPostfix]
    static void Postfix(CreatureAnimator __instance, AnimState state)
    {
        if (__instance != _necrobinderAnimator) return;
        if (!SkinRegistry.IsUsingSkin(ModelDb.Character<Necrobinder>().Id, "seesmakoto")) return;
        if (state.Id == "cast")
        {
            castAnim(0);
        }
        else if (state.Id == "cast_mighty") castAnim(0.5);
    }

    static void castAnim(double delays)
    {
        if (delays <= 0)
        {
            playAnim();
            return;
        }
        
        if (Engine.GetMainLoop() is SceneTree tree)
            tree.CreateTimer(delays).Timeout += playAnim;
    }

    static void playAnim()
    {
        MktAudio.Sound.Play(volumeMult: (float) MktConfig.EvokerSfxVolume / 100);
        if (_necrobinderCreature != null) VfxHelper.PlayPersonaVfx(_necrobinderCreature);
    }
}

[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.GenerateAnimator))]
internal static class TagAnimatorWithCreature
{
    private static bool Prepare() => Mkt_init.IsBeta();

    [HarmonyPostfix]
    private static void Postfix(CharacterModel __instance, Creature creature, CreatureAnimator __result)
        => EvokerSfx.TagAnimator(__instance, __result, creature);
}
