using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

[HarmonyPatch]
public class CharacterIconPatches
{
    [HarmonyPatch(typeof(CharacterModel), "get_IconTexture")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool charIconReplace(CharacterModel __instance, ref Texture2D __result)
    {
        string charId = __instance.Id.Entry.ToLower();
        Texture2D? skinIcon = SkinRegistry.textureResolve(charId, skin => skin.CharacterIcon);
        if (skinIcon == null) return true;
        __result = skinIcon;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_IconOutlineTexture")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool charOutlineIconReplace(CharacterModel __instance, ref Texture2D __result)
    {
        string charId = __instance.Id.Entry.ToLower();
        Texture2D? skinOutline = SkinRegistry.textureResolve(charId, skin => skin.CharacterIconOutline);
        if (skinOutline == null) return true;
        __result = skinOutline;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_IconPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool charIconSceneReplace(CharacterModel __instance, ref string __result)
    {
        string charId = __instance.Id.Entry.ToLower();
        string? skinPath = SkinRegistry.PathResolve(charId, skin => skin.IconScene);
        if (string.IsNullOrWhiteSpace(skinPath)) return true;
        __result = skinPath;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_MapMarkerPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool mapMarkerReplace(CharacterModel __instance, ref string __result)
    {
        string charId = __instance.Id.Entry.ToLower();
        string? skinPath = SkinRegistry.PathResolve(charId, skin => skin.MapMarker);
        if (string.IsNullOrWhiteSpace(skinPath)) return true;
        __result = skinPath;
        return false;
    }
}