using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

[HarmonyPatch]
public class CardNEnergyPatches
{
    [HarmonyPatch(typeof(CardPoolModel), "get_FrameMaterialPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CardFrameReplace(CardPoolModel __instance, ref string __result)
    {
        string charId = __instance.Title.ToLower();
        if(!SkinRegistry.GetAllCharacterIds().Contains(charId)) return true;
        //modEntry.Logger.Info($"CardPool --> {charId}");
        string? skinPath = SkinRegistry.PathResolve(charId, skin => skin.CardFrameMaterial);
        if (string.IsNullOrEmpty(skinPath)) return true;
        __result = skinPath;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_TrailPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CardTrailPath(CharacterModel __instance, ref string __result)
    {
        string charId = __instance.Id.Entry.ToLower();
        string? skinPath = SkinRegistry.PathResolve(charId, skin => skin.CardTrail);
        if (string.IsNullOrWhiteSpace(skinPath)) return true;
        __result = skinPath;
        return false;
    }

    private static void SetEnergyLayer(NEnergyCounter counter, string nodePath, Texture2D? newTexture)
    {
        if (newTexture == null) return;
        TextureRect? textureNode = counter.GetNodeOrNull<TextureRect>(nodePath);
        if (textureNode == null) return;
        textureNode.Texture = newTexture;
    }
    
    private static readonly AccessTools.FieldRef<NEnergyCounter, Player> PlayerField = AccessTools.FieldRefAccess<NEnergyCounter, Player>("_player");

    [HarmonyPatch(typeof(NEnergyCounter), "_Ready")]
    [HarmonyPostfix]
    private static void EnergyCounterReplace(NEnergyCounter __instance)
    {
        Player player = PlayerField.Invoke(__instance);
        string charId = player.Character.Id.Entry.ToLower();
        if (string.IsNullOrWhiteSpace(charId)) return;
        if (SkinRegistry.GetActiveSkin(charId).IsDefault) return;
        modEntry.Logger.Info("EnergyCounterReplace - Trying to set layers");
        SetEnergyLayer(__instance, "Layers/Layer1", SkinRegistry.textureResolve(charId, skin => skin.EnergyLayer1));
        SetEnergyLayer(__instance, "Layers/RotationLayers/Layer2", SkinRegistry.textureResolve(charId, skin => skin.EnergyLayer2));
        SetEnergyLayer(__instance, "Layers/RotationLayers/Layer3", SkinRegistry.textureResolve(charId, skin => skin.EnergyLayer3));
        SetEnergyLayer(__instance, "Layers/Layer4", SkinRegistry.textureResolve(charId, skin => skin.EnergyLayer4));
        SetEnergyLayer(__instance, "Layers/Layer5", SkinRegistry.textureResolve(charId, skin => skin.EnergyLayer5));
    }

    [HarmonyPatch(typeof(EnergyIconHelper), "GetPath", new[] { typeof(string) })]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool EnergyIconHelperReplace(string prefix, ref string __result)
    {
        string charId = prefix.ToLower();
        if (!SkinRegistry.GetAllCharacterIds().Contains(charId)) return true;
        string? skinPath = SkinRegistry.PathResolve(charId, skin => skin.EnergyIcon);
        if (string.IsNullOrWhiteSpace(skinPath)) return true;
        __result = skinPath;
        return false;
    }

    [HarmonyPatch(typeof(CardPoolModel), "get_EnergyIconPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CardEnergyReplace(CardPoolModel __instance, ref string __result)
    {
        string charId = __instance.Title.ToLower();
        if(!SkinRegistry.GetAllCharacterIds().Contains(charId)) return true;
        string? skinPath = SkinRegistry.PathResolve(charId, skin => skin.EnergyIcon);
        if (string.IsNullOrWhiteSpace(skinPath)) return true;
        __result = skinPath;
        return false;
    }
}

// Dynamically apply energy patches to every character's cardpool and energy
[HarmonyPatch]
public static class EnergyLabelOutlineColorReplace
{
    static IEnumerable<MethodBase> TargetMethods()
    {
        return AccessTools.AllTypes()
            .Where(t => !t.IsAbstract && typeof(CharacterModel).IsAssignableFrom(t))
            .Select(t => t.GetMethod("get_EnergyLabelOutlineColor", BindingFlags.Public | BindingFlags.Instance))
            .Where(m => m != null)
            .Cast<MethodBase>();
    }

    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool Prefix(CharacterModel __instance, ref Color __result)
    {
        string charId = __instance.Id.Entry.ToLower();
        Color? skinColor = SkinRegistry.colResolve(charId, skin => skin.EnergyLabelOutlineColor);
        if (skinColor == null) return true;
        __result = skinColor.Value;
        return false;
    }
}

[HarmonyPatch]
public static class CardLabelOutlineColorReplace
{
    static IEnumerable<MethodBase> TargetMethods()
    {
        return AccessTools.AllTypes()
            .Where(t => !t.IsAbstract && typeof(CardPoolModel).IsAssignableFrom(t))
            .Select(t => t.GetMethod("get_EnergyOutlineColor", BindingFlags.Public | BindingFlags.Instance))
            .Where(m => m != null)
            .Cast <MethodBase>();
    }

    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool Prefix(CardPoolModel __instance, ref Color __result)
    {
        string charId = __instance.Title.ToLower();
        if (!SkinRegistry.GetAllCharacterIds().Contains(charId)) return true;
        Color? skinColor = SkinRegistry.colResolve(charId, skin => skin.EnergyOutlineColor);
        if (skinColor == null) return true;
        __result = skinColor.Value;
        return false;
    }
}