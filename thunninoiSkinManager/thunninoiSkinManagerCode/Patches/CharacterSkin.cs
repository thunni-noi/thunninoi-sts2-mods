using System.Reflection;
using BaseLib.Utils.NodeFactories;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.RestSite;

namespace thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

public abstract class CharacterSkin
{
    public virtual ModelId? TargetCharId => null;

    public virtual string? CombatVisual => null;
    public virtual string? MerchantVisual => null;
    public virtual string? RestVisual => null;

    public virtual string? CharacterSelectBg => null;
    public virtual string? CharacterSelectPortrait => null;
    public virtual string? CharacterSelectTransition => null;

    public virtual string? CharacterIcon => null;
    public virtual string? CharacterIconOutline => null;
    public virtual string? CharacterIconScene => null;
    public virtual string? CharacterMapMarker => null;
    
    public virtual string? CardFrameMaterial => null;
    public virtual string? CardTrail => null;

    public virtual string? EnergyIcon => null;
    public virtual string[]? EnergyLayers => null;
    public virtual Color? EnergyLabelColor => null;
    public virtual Color? EnergyLabelOutlineColor => null;

    public virtual string? HandPoint => null;
    public virtual string? HandRock => null;
    public virtual string? HandPaper => null;
    public virtual string? HandScissors => null;
}

public partial class CharacterSkin<T> : CharacterSkin where T : CharacterModel
{
    public override ModelId TargetCharId => ModelDb.GetId<T>();
}

[HarmonyPatch]
public class CharacterSkinPatches
{
    [HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.CreateVisuals))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool combatVisualsReplace(CharacterModel __instance, ref NCreatureVisuals __result)
    {
        ModelId charId = __instance.Id;
        string? skinVisualPath = SkinRegistry.ResolvePath(charId, skin => skin.CharacterSkinData?.CombatVisual);
        if (string.IsNullOrWhiteSpace(skinVisualPath)) return true;
        PackedScene? scene = PreloadManager.Cache.GetScene(skinVisualPath);
        NCreatureVisuals visuals = NodeFactory<NCreatureVisuals>.CreateFromScene(scene);
        __result = visuals;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_RestSiteAnimPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool restVisualReplace(CharacterModel __instance, ref string __result)
    {
        ModelId charId = __instance.Id;
        string? skinPath = SkinRegistry.ResolvePath(charId, skin => skin.CharacterSkinData?.RestVisual);
        if (string.IsNullOrWhiteSpace(skinPath)) return true;
        __result = skinPath;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_MerchantAnimPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool merchanVisualReplace(CharacterModel __instance, ref string __result)
    {
        ModelId charId = __instance.Id;
        string? skinPath = SkinRegistry.ResolvePath(charId, skin => skin.CharacterSkinData?.MerchantVisual);
        if (string.IsNullOrWhiteSpace(skinPath)) return true;
        __result = skinPath;
        return false;
    }
}

[HarmonyPatch]
public class CharacterArmPatches
{
    private static bool ArmReplace(CharacterModel __instance, Func<SkinData, string?> selector, ref string __result)
    {
        ModelId charId = __instance.Id;
        if (!SkinRegistry.ResolveConfig(charId, SkinData.SkinConfigKey.UseHands)) return true;
        string? armPath = SkinRegistry.ResolvePath(charId, selector);
        if (string.IsNullOrWhiteSpace(armPath)) return true;
        __result = armPath;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_ArmPointingTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool ArmPointReplace(CharacterModel __instance, ref string __result) =>
        ArmReplace(__instance, s => s.CharacterSkinData?.HandPoint, ref __result);
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmRockTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool ArmRockReplace(CharacterModel __instance, ref string __result) =>
        ArmReplace(__instance, s => s.CharacterSkinData?.HandRock, ref __result);
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmPaperTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool ArmPaperReplace(CharacterModel __instance, ref string __result) =>
        ArmReplace(__instance, s => s.CharacterSkinData?.HandPaper, ref __result);
    
    [HarmonyPatch(typeof(CharacterModel), "get_ArmScissorsTexturePath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool ArmScissorsReplace(CharacterModel __instance, ref string __result) =>
        ArmReplace(__instance, s => s.CharacterSkinData?.HandScissors, ref __result);
}

[HarmonyPatch]
public class CharacterSelectPatches
{

    [HarmonyPatch(typeof(CharacterModel), "get_CharacterSelectBg")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CharacterSelectReplace(CharacterModel __instance, ref string __result)
    {
        ModelId charId = __instance.Id;
        string? skinBgPath = SkinRegistry.ResolvePath(charId, s => s.CharacterSkinData?.CharacterSelectBg);
        if (string.IsNullOrWhiteSpace(skinBgPath)) return true;
        __result = skinBgPath;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_CharacterSelectIconPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CharacterSelectPortraitReplace(CharacterModel __instance, ref string __result)
    {
        ModelId charId = __instance.Id;
        string? skinPortraitPath = SkinRegistry.ResolvePath(charId, s => s.CharacterSkinData?.CharacterSelectPortrait);
        if (string.IsNullOrWhiteSpace(skinPortraitPath)) return true;
        __result = skinPortraitPath;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_CharacterSelectTransitionPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CharacterSelectTransitionReplace(CharacterModel __instance, ref string __result)
    {
        ModelId charId = __instance.Id;
        string? skinPath = SkinRegistry.ResolvePath(charId, skin => SkinRegistry.ResolvePath(charId, s => s.CharacterSkinData?.CharacterSelectTransition));
        if (string.IsNullOrWhiteSpace(skinPath)) return true;
        __result = skinPath;
        return false;
    }
}

[HarmonyPatch]
public class CharacterIconPatches
{
    [HarmonyPatch(typeof(CharacterModel), "get_IconTexture")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool charIconReplace(CharacterModel __instance, ref Texture2D __result)
    {
        ModelId charId = __instance.Id;
        Texture2D? skinIcon = SkinRegistry.ResolveTexture(charId, skin => skin.CharacterSkinData?.CharacterIcon);
        if (skinIcon == null) return true;
        __result = skinIcon;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_IconOutlineTexture")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool charOutlineIconReplace(CharacterModel __instance, ref Texture2D __result)
    {
        ModelId charId = __instance.Id;
        Texture2D? skinOutline = SkinRegistry.ResolveTexture(charId, skin => skin.CharacterSkinData?.CharacterIconOutline);
        if (skinOutline == null) return true;
        __result = skinOutline;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_IconPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool charIconSceneReplace(CharacterModel __instance, ref string __result)
    {
        ModelId charId = __instance.Id;
        string? skinPath = SkinRegistry.ResolvePath(charId, skin => skin.CharacterSkinData?.CharacterIconScene);
        if (string.IsNullOrWhiteSpace(skinPath)) return true;
        __result = skinPath;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_MapMarkerPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool mapMarkerReplace(CharacterModel __instance, ref string __result)
    {
        ModelId charId = __instance.Id;
        string? skinPath = SkinRegistry.ResolvePath(charId, skin => skin.CharacterSkinData?.CharacterMapMarker);
        if (string.IsNullOrWhiteSpace(skinPath)) return true;
        __result = skinPath;
        return false;
    }
}

[HarmonyPatch]
public class CardNEnergyPatches
{
    [HarmonyPatch(typeof(CardPoolModel), "get_FrameMaterialPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CardFrameReplace(CardPoolModel __instance, ref string __result)
    {
        string charName = __instance.Title.ToUpper();
        ModelId charId = new ModelId("CHARACTER", charName);
        if (!SkinRegistry.GetAllActiveSkins().ContainsKey(charId)) return true;
        if (!SkinRegistry.ResolveConfig(charId, SkinData.SkinConfigKey.UseCardFrame)) return true;
        string? skinPath = SkinRegistry.ResolvePath(charId, skin => skin.CharacterSkinData?.CardFrameMaterial);
        if (string.IsNullOrEmpty(skinPath)) return true;
        __result = skinPath;
        return false;
    }

    [HarmonyPatch(typeof(CharacterModel), "get_TrailPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CardTrailPath(CharacterModel __instance, ref string __result)
    {
        ModelId charId = __instance.Id;
        if (!SkinRegistry.ResolveConfig(charId, SkinData.SkinConfigKey.UseCardFrame)) return true;
        string? skinPath = SkinRegistry.ResolvePath(charId, skin => skin.CharacterSkinData?.CardTrail);
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
        Player? player = PlayerField.Invoke(__instance);
        ModelId? charId = player.Character.Id;
        if (!SkinRegistry.ResolveConfig(charId, SkinData.SkinConfigKey.UseEnergy)) return;
        //if (SkinRegistry.GetActiveSkin(charId).IsDefault) return;
        modEntry.Logger.Info("EnergyCounterReplace - Trying to set layers");
        SetEnergyLayer(__instance, "Layers/Layer1", SkinRegistry.ResolveTexture(charId, skin => skin.CharacterSkinData?.EnergyLayers?[0]));
        SetEnergyLayer(__instance, "Layers/RotationLayers/Layer2", SkinRegistry.ResolveTexture(charId, skin => skin.CharacterSkinData?.EnergyLayers?[1]));
        SetEnergyLayer(__instance, "Layers/RotationLayers/Layer3", SkinRegistry.ResolveTexture(charId, skin => skin.CharacterSkinData?.EnergyLayers?[2]));
        SetEnergyLayer(__instance, "Layers/Layer4", SkinRegistry.ResolveTexture(charId, skin => skin.CharacterSkinData?.EnergyLayers?[3]));
        SetEnergyLayer(__instance, "Layers/Layer5", SkinRegistry.ResolveTexture(charId, skin => skin.CharacterSkinData?.EnergyLayers?[4]));
    }

    [HarmonyPatch(typeof(EnergyIconHelper), "GetPath", new[] { typeof(string) })]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool EnergyIconHelperReplace(string prefix, ref string __result)
    {
        string charName = prefix.ToUpper();
        ModelId charId = new ModelId("CHARACTER", charName);
        if (!SkinRegistry.ResolveConfig(charId, SkinData.SkinConfigKey.UseEnergy)) return true;
        if (!SkinRegistry.GetAllActiveSkins().ContainsKey(charId)) return true;
        string? skinPath = SkinRegistry.ResolvePath(charId, skin => skin.CharacterSkinData?.EnergyIcon);
        if (string.IsNullOrWhiteSpace(skinPath)) return true;
        __result = skinPath;
        return false;
    }

    [HarmonyPatch(typeof(CardPoolModel), "get_EnergyIconPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CardEnergyReplace(CardPoolModel __instance, ref string __result)
    {
        string charName = __instance.Title.ToUpper();
        ModelId charId = new ModelId("CHARACTER", charName);
        if (!SkinRegistry.ResolveConfig(charId, SkinData.SkinConfigKey.UseEnergy)) return true;
        if (!SkinRegistry.GetAllActiveSkins().ContainsKey(charId)) return true;
        string? skinPath = SkinRegistry.ResolvePath(charId, skin => skin.CharacterSkinData?.EnergyIcon);
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
        ModelId charId = __instance.Id;
        if (!SkinRegistry.ResolveConfig(charId, SkinData.SkinConfigKey.UseEnergy)) return true;
        Color? skinColor = SkinRegistry.ResolveColor(charId, skin => skin.CharacterSkinData?.EnergyLabelColor);
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
        string charName = __instance.Title.ToUpper();
        ModelId charId = new ModelId("CHARACTER", charName);
        if (!SkinRegistry.ResolveConfig(charId, SkinData.SkinConfigKey.UseEnergy)) return true;
        if (!SkinRegistry.GetAllActiveSkins().ContainsKey(charId)) return true;
        Color? skinColor = SkinRegistry.ResolveColor(charId, skin => skin.CharacterSkinData.EnergyLabelOutlineColor);
        if (skinColor == null) return true;
        __result = skinColor.Value;
        return false;
    }
}

[HarmonyPatch]
public static class EntryAnimationAdd
{
    static IEnumerable<MethodBase> TargetMethods()
    {
        return AccessTools.AllTypes()
            .Where(t => !t.IsAbstract && typeof(CharacterModel).IsAssignableFrom(t))
            .Select(t => t.GetMethod("GenerateAnimator"))
            .Where(m => m != null)
            .Cast<MethodBase>();
    }

    [HarmonyPostfix]
    public static void AddEntry(MegaSprite controller,CharacterModel __instance, ref CreatureAnimator __result)
    {
        modEntry.Logger.Info(__instance.Id.Entry);
        if (!controller.HasAnimation("entry")) return;
        FieldInfo? currentState = typeof(CreatureAnimator).GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);
        AnimState? existedIdle = currentState.GetValue(__result) as AnimState;
        if (existedIdle == null) return;

        AnimState entryState = new AnimState("entry")
        {
            NextState = existedIdle
        };
        
        __result.AddAnyState("Entry", entryState);
        currentState.SetValue(__result, entryState);

        var animationState = controller.GetAnimationState();
        animationState.SetAnimation("entry", false, 0);
        animationState.AddAnimation(existedIdle.Id, 0f, true, 0);
    }
}