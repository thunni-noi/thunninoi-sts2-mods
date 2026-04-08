using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace ChenIronclad.ChenIroncladCode.patches;

[HarmonyPatch]
public class EnergyPatch
{
    private const string CharacterEnergyIcon = "res://ChenIronclad/assets/ui/energy/chen_energy_icon.png";
    private static Color _cardOutlineColor = new Color(0f / 255f, 0f / 255f, 170f / 255f, 1f);
    private static string CharacterOrbLayer(string layer) => $"res://ChenIronclad/assets/ui/energy/chen_orb_layer_{layer}.png";
    private static readonly AccessTools.FieldRef<NEnergyCounter, Player> PlayerField = AccessTools.FieldRefAccess<NEnergyCounter, Player>("_player");

    private static void SetLayerTexture(NEnergyCounter counter, string nodePath, string texturePath)
    {
        if (!ResourceLoader.Exists(texturePath))
        {
            Log.Error($"[CHEN] Cannot find texture : {texturePath}");
            return;
        }
        
        TextureRect node = ((Node)counter).GetNodeOrNull<TextureRect>(nodePath);
        Texture2D newTexture = ResourceLoader.Load<Texture2D>(texturePath);
        if (node != null && newTexture != null) node.Texture = newTexture;
    }

    [HarmonyPatch(typeof(NEnergyCounter), "_Ready")]
    [HarmonyPostfix]
    private static void EnergyCounterPatch(NEnergyCounter __instance)
    {
        if (!ChenConfig.UseChenEnergy) return;
        Player player = PlayerField.Invoke(__instance);
        if (player?.Character is not Ironclad) return;
        
        SetLayerTexture(__instance, "Layers/Layer1", CharacterOrbLayer("1"));
        SetLayerTexture(__instance, "Layers/RotationLayers/Layer2", CharacterOrbLayer("2"));
        SetLayerTexture(__instance, "Layers/RotationLayers/Layer3", CharacterOrbLayer("3"));
        SetLayerTexture(__instance, "Layers/Layer4", CharacterOrbLayer("4"));
        SetLayerTexture(__instance, "Layers/Layer5", CharacterOrbLayer("5"));
    }

    [HarmonyPatch(typeof(EnergyIconHelper), "GetPath", new Type[] { typeof(string) })]
    [HarmonyPrefix]
    private static bool EnergyIconHelperPatch(string prefix, ref string __result)
    {
        if (!ChenConfig.UseChenEnergy) return true;
        if (!string.Equals(prefix, "ironclad", StringComparison.OrdinalIgnoreCase)) return true;
        if (!ResourceLoader.Exists(CharacterEnergyIcon))
        {
            Log.Error($"[CHEN] Cannot find resource : {CharacterEnergyIcon}");
            return true;
        }

        __result = CharacterEnergyIcon;
        return false;
    }

    [HarmonyPatch(typeof(CardPoolModel), "get_EnergyIconPath")]
    [HarmonyPrefix]
    private static bool CardEnergyIconPatch(CardPoolModel __instance, ref string __result)
    {
        if (!ChenConfig.UseChenEnergy) return true;
        if (!(__instance is IroncladCardPool)) return true;
        if (!ResourceLoader.Exists(CharacterEnergyIcon))
        {
            Log.Error($"[CHEN] Cannot find resource : {CharacterEnergyIcon}");
            return true;
        }
        __result = CharacterEnergyIcon;
        return false;
    }

    [HarmonyPatch(typeof(Ironclad), "get_EnergyLabelOutlineColor")]
    [HarmonyPrefix]
    private static bool EnergyCounterLabelOutlinePatch(ref Color __result)
    {
        if (!ChenConfig.UseChenEnergy) return true;
        __result = _cardOutlineColor;
        return false;
    }

    [HarmonyPatch(typeof(IroncladCardPool), "get_EnergyOutlineColor")]
    [HarmonyPrefix]
    private static bool EnergyCardLabelOutlinePatch(ref Color __result)
    {
        if (!ChenConfig.UseChenEnergy) return true;
        __result = _cardOutlineColor;
        return false;
    }




    
}