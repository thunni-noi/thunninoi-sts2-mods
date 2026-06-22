using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Relics;

namespace thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

public abstract class RelicSkin
{
    public virtual ModelId? TargetRelicId => null;
    public virtual string? PackedIconPath => null;
    public virtual string? PackedIconOutlinePath => null;
    public virtual string? BigIconPath => null;
}

public abstract class RelicSkin<T> : RelicSkin where T : RelicModel
{
    public override ModelId? TargetRelicId => ModelDb.GetId<T>();
}

[HarmonyPatch(typeof(RelicModel), nameof(RelicModel.PackedIconPath), MethodType.Getter)]
class RelicIconSkin
{
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    public static bool PackedIconSkin(RelicModel __instance, ref string? __result)
    {
        RelicSkin? skinData = SkinRegistry.ResolveRelic(__instance.Id);
        if (skinData?.PackedIconPath is string iconPath)
        {
            __result = iconPath;
            return false;
        }
        return true;
    }
}

[HarmonyPatch(typeof(RelicModel), "PackedIconOutlinePath", MethodType.Getter)]
class RelicIconOutlineSkin
{
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    public static bool PackedIconOutlineSkin(RelicModel __instance, ref string? __result)
    {
        RelicSkin? skinData = SkinRegistry.ResolveRelic(__instance.Id);
        if (skinData?.PackedIconOutlinePath is string outlinePath)
        {
            __result = outlinePath;
            return false;
        }

        return true;
    }
}

[HarmonyPatch(typeof(RelicModel), "BigIconPath", MethodType.Getter)]
class RelicBigIconSkin
{
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    public static bool BigIconSkin(RelicModel __instance, ref string? __result)
    {
        RelicSkin? skinData = SkinRegistry.ResolveRelic(__instance.Id);
        if (skinData?.BigIconPath is string bigIconPath)
        {
            __result = bigIconPath;
            return false;
        }

        return true;
    }
}

[HarmonyPatch(typeof(NRelic), "Reload")]
class BigIconTexture
{
    private static readonly FieldInfo RelicModel = AccessTools.Field(typeof(NRelic), "_model");
    
    [HarmonyPostfix]
    public static void BigIconSkin(NRelic __instance)
    {
        if (!__instance.IsNodeReady()) return;
        if (RelicModel.GetValue(__instance) is not RelicModel model) return;
        if (SkinRegistry.ResolveRelic(model.Id)?.BigIconPath is string bigIconPath)
        {
            __instance.Icon.Texture = PreloadManager.Cache.GetAsset<Texture2D>(bigIconPath);
        }
    }
}