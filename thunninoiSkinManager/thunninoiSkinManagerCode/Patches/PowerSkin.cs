using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Models;

namespace thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

public abstract class PowerSkin
{
    public virtual ModelId? TargetPowerId => null;
    public virtual string? CustomIconPath => null;
    public virtual string? CustomBigIconPath => null;
}

public abstract class PowerSkin<T> : PowerSkin where T : PowerModel
{
    public override ModelId TargetPowerId => ModelDb.GetId<T>();
}

[HarmonyPatch(typeof(PowerModel), "get_Icon")]
public static class PowerIcon
{
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool PowerIconReplace(PowerModel __instance, ref Texture2D __result)
    {
        PowerSkin? powerSkin = SkinRegistry.ResolvePower(__instance.Id);
        if (powerSkin is PowerSkin)
        {
            if (powerSkin.CustomIconPath != null)
            {
                __result = PreloadManager.Cache.GetTexture2D(powerSkin.CustomIconPath);
                return false;
            }
        }
        return true;
    }
}

[HarmonyPatch(typeof(PowerModel), "get_BigIcon")]
public static class PowerBigIcon
{
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool Prefix(PowerModel __instance, ref Texture2D __result)
    {
        PowerSkin? powerSkin = SkinRegistry.ResolvePower(__instance.Id);
        if (powerSkin is PowerSkin)
        {
            if (powerSkin.CustomIconPath != null)
            {
                __result = PreloadManager.Cache.GetTexture2D(powerSkin.CustomIconPath);
                return false;
            }
        }

        return true;
    }
}



