using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;

namespace thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

// code taken and modified from baselib
public abstract class OrbSkin
{
    public virtual ModelId? TargetOrbId => null;
    public virtual string? CustomIconPath => null;
    public virtual string? CustomSpritePath => null;
    public virtual Color? CustomDarkenedColor => null;
    public virtual Node2D? CreateCustomSprite() => null;
}

public abstract class OrbSkin<T> : OrbSkin where T : OrbModel
{
    public override ModelId TargetOrbId => ModelDb.GetId<T>();
}

[HarmonyPatch]
class OrbSkinPatches
{
    internal static bool IsOrbEnabled()
    {
        return SkinRegistry.ResolveConfig(ModelDb.Character<Defect>().Id, SkinData.SkinConfigKey.UseDefectOrbs);
    }
    
    [HarmonyPatch(typeof(OrbModel), "IconPath", MethodType.Getter)]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    static bool IconPrefix(OrbModel __instance, ref string __result)
    {
        if (!IsOrbEnabled()) return true;
        ModelId orbId = __instance.Id;
        OrbSkin? curOrbSkin = SkinRegistry.ResolveOrb(orbId);
        if (curOrbSkin?.CustomIconPath is string orbIconPath)
        {
            __result = orbIconPath;
            return false;
        }
        return true;
    }
    
    [HarmonyPatch(typeof(OrbModel), "SpritePath", MethodType.Getter)]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    static bool SpritePrefix(OrbModel __instance, ref string __result)
    {
        if (!IsOrbEnabled()) return true;
        ModelId orbId = __instance.Id;
        OrbSkin? curOrbSkin = SkinRegistry.ResolveOrb(orbId);
        if (curOrbSkin?.CustomSpritePath is string path)
        {
            __result = path;
            return false;
        }
        return true;
    }
    
    [HarmonyPatch(typeof(OrbModel), nameof(OrbModel.CreateSprite))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    static bool CreateSpritePrefix(OrbModel __instance, ref Node2D __result)
    {
        if (!IsOrbEnabled()) return true;
        ModelId orbId = __instance.Id;
        OrbSkin? curOrbSkin = SkinRegistry.ResolveOrb(orbId);
        if (curOrbSkin?.CreateCustomSprite() is Node2D customSprite)
        {
            __result = customSprite;
            return false;
        }
        return true;
    }
}

[HarmonyPatch]
class OrbDarkenedColorSkin
{
    static IEnumerable<MethodBase> TargetMethods()
    {
        var methods = AccessTools.AllTypes()
            .Where(t => !t.IsAbstract && typeof(OrbModel).IsAssignableFrom(t))
            .Select(t => AccessTools.DeclaredPropertyGetter(t, "DarkenedColor"))
            .Where(m => m != null)
            .Cast<MethodBase>()
            .ToList();
        return methods;
    }

    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    static bool Prefix(OrbModel __instance, ref Color __result)
    {
        if (!OrbSkinPatches.IsOrbEnabled()) return true;
        ModelId orbId = __instance.Id;
        OrbSkin? curOrbSkin = SkinRegistry.ResolveOrb(orbId);
        if (curOrbSkin?.CustomDarkenedColor is Color col)
        {
            __result = col;
            return false;
        }
        return true;
    }
}