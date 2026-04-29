using CEdefect.CEdefectCode.SkinManager;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Relics;

namespace CEdefect.CEdefectCode.Patches;

[HarmonyPatch]
public class CE_RelicsPatches
{
    [HarmonyPatch(typeof(RelicModel), "get_Icon")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_RelicIconPatch(RelicModel __instance, ref Texture2D __result)
    {
        //if (!YourConfig.UseCustomRelicIcons) return true;
        string icon_path = "";
        
        if (__instance is CrackedCore)
        {
            icon_path = SharedResources.CrackedCoreIcon;
        } else if(__instance is InfusedCore)
        {
            icon_path = SharedResources.InfusedCoreIcon;
        }
        
        if (string.IsNullOrWhiteSpace(icon_path)) return true;
        Texture2D? tex = ResourceLoader.Load<Texture2D>(icon_path, null, ResourceLoader.CacheMode.Reuse);
        if (tex == null) return true;
        __result = tex;
        return false;
    }
    
    [HarmonyPatch(typeof(RelicModel), "get_BigIconPath")]
    [HarmonyPrefix]
    private static bool CE_RelicBigIconPatch(RelicModel __instance, ref string __result)
    {
        //if (!YourConfig.UseCustomRelicIcons) return true;
        string icon_path = "";
        
        if (__instance is CrackedCore)
        {
            icon_path = SharedResources.CrackedCoreIcon;
        } else if(__instance is InfusedCore)
        {
            icon_path = SharedResources.InfusedCoreIcon;
        }
        
        if (string.IsNullOrWhiteSpace(icon_path)) return true;
        __result = icon_path;
        return false;
    }
    
    [HarmonyPatch(typeof(RelicModel), "get_IconOutline")]
    [HarmonyPrefix]
    private static bool CE_RelicOutlinePatch(RelicModel __instance, ref Texture2D __result)
    {
        //if (!YourConfig.UseCustomRelicIcons) return true;
        string icon_path = "";
        
        if (__instance is CrackedCore)
        {
            icon_path = SharedResources.CrackedCoreOutline;
        } else if(__instance is InfusedCore)
        {
            icon_path = SharedResources.InfusedCoreOutline;
        }
        
        if (string.IsNullOrWhiteSpace(icon_path)) return true;
        Texture2D? tex = ResourceLoader.Load<Texture2D>(icon_path, null, ResourceLoader.CacheMode.Reuse);
        if (tex == null) return true;
        __result = tex;
        return false;
    }

    [HarmonyPatch(typeof(NRelic), "Reload")]
    [HarmonyPostfix]
    private static void ReliceloadPostfix(NRelic __instance)
    {
        if (!__instance.IsNodeReady()) return;
        string icon_path = "";
        
        if (__instance is CrackedCore)
        {
            icon_path = SharedResources.CrackedCoreIcon;
        } else if(__instance is InfusedCore)
        {
            icon_path = SharedResources.InfusedCoreIcon;
        }

        if (string.IsNullOrWhiteSpace(icon_path)) return;
        Texture2D? tex = ResourceLoader.Load<Texture2D>(icon_path, null, ResourceLoader.CacheMode.Reuse);
        if (tex != null) __instance.Icon.Texture = tex;
        
    }
}