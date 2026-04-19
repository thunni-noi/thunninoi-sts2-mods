using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace InesSilent.InesSilentCode;

[HarmonyPatch]
public class InesEnergyPatches
{
     private const string CharacterEnergyIcon = "res://InesSilent/assets/ui/energy/ines_energy_icon.png";
    private static string CharacterOrbLayer(string layer) => $"res://InesSilent/assets/ui/energy/ines_orb_layer_{layer}.png";
    private static readonly AccessTools.FieldRef<NEnergyCounter, Player> PlayerField = AccessTools.FieldRefAccess<NEnergyCounter, Player>("_player");

    private static void SetLayerTexture(NEnergyCounter counter, string nodePath, string texturePath)
    {
        if (!InesUtils.ResourceAvailable(texturePath)) return;
        
        TextureRect node = ((Node)counter).GetNodeOrNull<TextureRect>(nodePath);
        Texture2D newTexture = ResourceLoader.Load<Texture2D>(texturePath);
        if (node != null && newTexture != null) node.Texture = newTexture;
    }

    [HarmonyPatch(typeof(NEnergyCounter), "_Ready")]
    [HarmonyPostfix]
    private static void Ines_EnergyCounterPatch(NEnergyCounter __instance)
    {
        if (!InesConfig.UseInesEnergy) return;
        Player player = PlayerField.Invoke(__instance);
        if (player?.Character is not Silent) return;
        
        SetLayerTexture(__instance, "Layers/Layer1", CharacterOrbLayer("1"));
        SetLayerTexture(__instance, "Layers/RotationLayers/Layer2", CharacterOrbLayer("2"));
        SetLayerTexture(__instance, "Layers/RotationLayers/Layer3", CharacterOrbLayer("3"));
        SetLayerTexture(__instance, "Layers/Layer4", CharacterOrbLayer("4"));
        SetLayerTexture(__instance, "Layers/Layer5", CharacterOrbLayer("5"));
    }

    [HarmonyPatch(typeof(EnergyIconHelper), "GetPath", new Type[] { typeof(string) })]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_EnergyIconHelperPatch(string prefix, ref string __result)
    {
        if (!InesConfig.UseInesEnergy) return true;
        if (!string.Equals(prefix, "silent", StringComparison.OrdinalIgnoreCase)) return true;
        if (!InesUtils.ResourceAvailable(CharacterEnergyIcon)) return true;

        __result = CharacterEnergyIcon;
        return false;
    }

    [HarmonyPatch(typeof(CardPoolModel), "get_EnergyIconPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_CardEnergyIconPatch(CardPoolModel __instance, ref string __result)
    {
        if (!InesConfig.UseInesEnergy) return true;
        if (!(__instance is SilentCardPool)) return true;
        if (!InesUtils.ResourceAvailable(CharacterEnergyIcon)) return true;
        
        __result = CharacterEnergyIcon;
        return false;
    }

    [HarmonyPatch(typeof(Silent), "get_EnergyLabelOutlineColor")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_EnergyCounterLabelOutlinePatch(ref Color __result)
    {
        if (!InesConfig.UseInesEnergy) return true;
        __result = new Color(0.51f, 0f, 0f, 1f);
        return false;
    }
    
    [HarmonyPatch(typeof(SilentCardPool), "get_EnergyOutlineColor")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    private static bool Ines_EnergyCardLabelOutlinePatch(ref Color __result)
    {
        if (!InesConfig.UseInesEnergy) return true;
        __result = new Color(0.51f, 0f, 0f, 1f);
        return false;
    }
}