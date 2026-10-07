using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes;
using thunninoiSkinManager.thunninoiSkinManagerCode;

namespace MakotoNecrobinder.MakotoNecrobinderCode.Patches;

[HarmonyPatch(typeof(NTransition))]
public class transition
{
    private static readonly MethodInfo? InTransitionSetter =
        AccessTools.PropertySetter(typeof(NTransition), nameof(NTransition.InTransition));

    private static void SetInTransition(NTransition t, bool value) =>
        InTransitionSetter?.Invoke(t, new object[] { value });

    private static Control? _transitionVideo;

    // ---------------------------------------------------------------- FadeOut

    [HarmonyPatch(nameof(NTransition.FadeOut))]
    [HarmonyPrefix]
    private static bool FadeOutPrefix(NTransition __instance, string transitionPath, ref Task __result)
    {
        if (transitionPath != "res://materials/transitions/necrobinder_transition_mat.tres") return true;
        if (!MktConfig.UseMakotoTransition) return true;
        if (!SkinRegistry.IsUsingSkin(ModelDb.Character<Necrobinder>().Id, "seesmakoto")) return true;

        __result = PlayVideo(__instance);
        return false;
    }

    private static async Task PlayVideo(NTransition t)
    {
        Control video = PreloadManager.Cache.GetScene("res://MakotoNecrobinder/assets/sees/ui/transition/makoto_transition.tscn").Instantiate<Control>();
        VideoStreamPlayer player = video.GetNode<VideoStreamPlayer>("transitionVideo");
        _transitionVideo = video;

        SetInTransition(t, true);
        t.MouseFilter = Control.MouseFilterEnum.Stop;
        t.AddChild(video);

        player.Play();
        await t.ToSignal(player, VideoStreamPlayer.SignalName.Finished);

        //black cover
        ColorRect blackCover = new() { Name = "transitionCover", Color = Colors.Black, Modulate = new Color(1, 1, 1, 0) };
        blackCover.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        video.AddChild(blackCover);
        Tween tween = t.CreateTween();
        tween.TweenProperty(blackCover, "modulate:a", 1f, 0.2f);
        await t.ToSignal(tween, Tween.SignalName.Finished);

        player.QueueFree();
    }
    

    [HarmonyPatch(nameof(NTransition.FadeIn))]
    [HarmonyPrefix]
    private static bool FadeInPrefix(NTransition __instance, ref Task __result) => transitionOut(__instance, ref __result);

    [HarmonyPatch(nameof(NTransition.RoomFadeIn))]
    [HarmonyPrefix]
    private static bool RoomFadeInPrefix(NTransition __instance, ref Task __result) => transitionOut(__instance, ref __result);

    private static bool transitionOut(NTransition t, ref Task result)
    {
        if (_transitionVideo == null) return true;

        result = fadeOutTransition(t, _transitionVideo);
        return false;
    }

    private static async Task fadeOutTransition(NTransition t, Control video)
    {
        foreach (string field in new[] { "_simpleTransition", "_gradientTransition" })
        {
            Control? layer = Traverse.Create(t).Field(field).GetValue<Control>();
            if (layer == null) continue;
            Color c = layer.Modulate;
            c.A = 0f;
            layer.Modulate = c;
        }
        
        // fade cover out
        ColorRect black = video.GetNode<ColorRect>("transitionCover");

        Tween tween = t.CreateTween();
        tween.TweenProperty(black, "modulate:a", 0f, 0.5);
        await t.ToSignal(tween, Tween.SignalName.Finished);

        video.QueueFree();
        _transitionVideo = null;
        t.MouseFilter = Control.MouseFilterEnum.Ignore;
        SetInTransition(t, false);
    }
}