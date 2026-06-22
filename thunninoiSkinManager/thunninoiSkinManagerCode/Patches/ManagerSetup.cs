using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes;

namespace thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

[HarmonyPatch(typeof(OneTimeInitialization), nameof(OneTimeInitialization.ExecuteEssential))]
public class ManagerSetup
{
    private static bool _initialized = false;
    [HarmonyPostfix]
    private static void Postfix()
    {
        if (_initialized) return;
        modEntry.Logger.Info("Loading Db");
        SkinRegistry.SkinDbSetup();
        _initialized = true;
    }
}

[HarmonyPatch(typeof(SkinRegistry), nameof(SkinRegistry.SkinDbSetup))]
public static class FinalizeSkinDb
{
    [HarmonyFinalizer]
    private static void Finalizer()
    {
        SkinRegistry.finializeSetup();
    }
}