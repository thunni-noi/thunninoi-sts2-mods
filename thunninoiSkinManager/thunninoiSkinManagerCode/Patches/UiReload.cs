using System.Reflection;
using BaseLib.Patches.UI;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Relics;

namespace thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

// Reload UI as needed ; code taken from baselib
public interface CustomUiModel
{
    public void CreateCustomUi(Control toAdd);
}

static class UiReload
{
    [HarmonyPatch(typeof(NRelic), "Reload")]
    static class RelicUi
    {
        private static readonly FieldInfo RelicModel = AccessTools.Field(typeof(NRelic), "_model");
        
        [HarmonyFinalizer]
        static void Postfix(NRelic __instance)
        {
            if (RelicModel.GetValue(__instance) is not RelicModel model) return;
            Recreate(__instance, model);
        }
    }
    
    private static void Recreate(Node n, object? model)
    {
        foreach (var child in n.GetChildren())
        {
            if (child is not NTemporaryUi) continue;
            
            child.Name += "_TRASH";
            child.QueueFreeSafely();
        }

        if (model is not ICustomUiModel customUi) return;
        
        var tempNode = new NTemporaryUi();
        tempNode.Name = model.GetType().Name + "_TEMP";
        customUi.CreateCustomUi(tempNode);
        n.AddChild(tempNode);
        tempNode.Owner = n;
    }
}

internal partial class NTemporaryUi : Control;
