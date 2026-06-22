using System.Net.Mime;
using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

[HarmonyPatch(typeof(NShivThrowVfx), "ApplyTint")]
public class SilentShivColor
{
    // Silent's shiv color
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool Prefix(ref Color tint)
    {
        if (SkinRegistry.ResolveConfig(ModelDb.Character<Silent>().Id, SkinData.SkinConfigKey.SilentRecolorShiv))
        {   
            Color? shivColor = (SkinRegistry.ResolveColor(ModelDb.Character<Silent>().Id, s => s.ShivTintColor));
            if (shivColor.HasValue)
            {
                tint = shivColor.Value;
            }
        }
        return true;
    }
    
}