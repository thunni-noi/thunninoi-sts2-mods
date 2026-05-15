using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes;

namespace thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

[HarmonyPatch]
public class ManagerSetup
{
    [HarmonyPatch(typeof(NGame), "_Ready")]
    [HarmonyPostfix]
    private static void setup()
    {
        SkinRegistry.SkinDbSetup();
    }
}