using CEdefect.CEdefectCode.SkinManager;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

namespace CEdefect.CEdefectCode.Patches;

[HarmonyPatch]
public class CE_EnergyPatches
{
    private static readonly AccessTools.FieldRef<NEnergyCounter, Player> PlayerField = AccessTools.FieldRefAccess<NEnergyCounter, Player>("_player");

    private static void SetLayerTexture(NEnergyCounter counter, string nodePath, string texturePath)
    {
        if (!CE_Utils.ResourceAvailable(texturePath)) return;
        
        TextureRect node = ((Node)counter).GetNodeOrNull<TextureRect>(nodePath);
        Texture2D newTexture = ResourceLoader.Load<Texture2D>(texturePath);
        if (node != null && newTexture != null) node.Texture = newTexture;
    }

    [HarmonyPatch(typeof(NEnergyCounter), "_Ready")]
    [HarmonyPostfix]
    private static void CE_EnergyCounterPatch(NEnergyCounter __instance)
    {
        CE_Utils.Logger("Entering EnergyCounterPatch");
        if (!CE_Config.UseCivilightEnergy) return;
        Player player = PlayerField.Invoke(__instance);
        if (player?.Character is not Defect) return;
        CE_Utils.Logger("Patching EnergyCounterPatch");
        
        SetLayerTexture(__instance, "Layers/Layer1", SharedResources.EnergyLayer1);
        SetLayerTexture(__instance, "Layers/RotationLayers/Layer2", SharedResources.EnergyLayer2);
        SetLayerTexture(__instance, "Layers/RotationLayers/Layer3", SharedResources.EnergyLayer3);
        SetLayerTexture(__instance, "Layers/Layer4", SharedResources.EnergyLayer4);
        SetLayerTexture(__instance, "Layers/Layer5", SharedResources.EnergyLayer5);
    }
    
    [HarmonyPatch(typeof(CharacterModel), "get_TrailPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_CardTrailPatch(CharacterModel __instance, ref string __result)
    {
        if (!CE_Config.UseCivilightCardFrame) return true;
        if (!CE_Utils.IsDefect(__instance) || !CE_Utils.ResourceAvailable(SharedResources.CardTrailScene)) return true; 
        __result = SharedResources.CardTrailScene;
        return false;
    }
    
    [HarmonyPatch(typeof(CardPoolModel), "get_FrameMaterialPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool Ines_CardFramePatch(CardPoolModel __instance, ref string __result)
    {
        if (!CE_Config.UseCivilightCardFrame) return true;
        if (!((__instance) is DefectCardPool)) return true;
        __result = SharedResources.CardFramePath;
        return false;
    }

    [HarmonyPatch(typeof(Defect), "get_EnergyLabelOutlineColor")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_EnergyLabelOutlineColorPatch(ref Color __result)
    {
        if (!CE_Config.UseCivilightEnergy) return true;
        __result = SharedResources.energyColor;
        return false;
    }
    
    [HarmonyPatch(typeof(DefectCardPool), "get_EnergyOutlineColor")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_EnergyOutlineColorPatch(ref Color __result)
    {
        if(!CE_Config.UseCivilightEnergy) return true;
        __result = SharedResources.energyColor;
        return false;
    }
    
    [HarmonyPatch(typeof(EnergyIconHelper), "GetPath", new Type[] { typeof(string) })]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool Ines_EnergyIconHelperPatch(string prefix, ref string __result)
    {
        if(!CE_Config.UseCivilightEnergy) return true;
        if (!string.Equals(prefix, "defect", StringComparison.OrdinalIgnoreCase)) return true;

        __result = SharedResources.EnergyIcon;
        return false;
    }

    [HarmonyPatch(typeof(CardPoolModel), "get_EnergyIconPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool Ines_CardEnergyIconPatch(CardPoolModel __instance, ref string __result)
    {
        if(!CE_Config.UseCivilightEnergy) return true;
        if (!(__instance is DefectCardPool)) return true;
        
        __result = SharedResources.EnergyIcon;
        return false;
    }
    
}