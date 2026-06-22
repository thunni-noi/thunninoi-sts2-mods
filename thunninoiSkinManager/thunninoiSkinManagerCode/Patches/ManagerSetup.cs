using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes;

namespace thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

[HarmonyPatch]
public class ManagerSetup
{
    private static bool _initialized = false;
    
    [HarmonyPatch(typeof(OneTimeInitialization), nameof(OneTimeInitialization.ExecuteEssential))]
    [HarmonyPostfix]
    private static void setup()
    {
        if (_initialized) return;
        modEntry.Logger.Info("Loading Db");
        SkinRegistry.SkinDbSetup();
        _initialized = true;
    }
}