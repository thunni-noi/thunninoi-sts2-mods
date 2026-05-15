using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using thunninoiSkinManager.thunninoiSkinManagerCode;

namespace InesSilent.InesSilentCode;

[HarmonyPatch]
public class RegisterSkin
{
    [HarmonyPatch(typeof(SkinRegistry), nameof(SkinRegistry.SkinDbSetup))]
    [HarmonyPostfix]
    public static void RegisterInesSkin()
    {
        SkinData InesSkin = new SkinData("silent", "ines", "Ines");
        InesSkin.RegisterVisuals(
            "res://InesSilent/scenes/character/ines_default.tscn",
            "res://InesSilent/scenes/character/ines_merchant.tscn",
            "res://InesSilent/scenes/character/ines_rest_site.tscn");
        InesSkin.RegisterCharacterSelect(
            "res://InesSilent/scenes/ui/char_select_bg_ines.tscn",
            "res://InesSilent/assets/ui/characterSelect/ines_portrait.png");
        InesSkin.RegisterIcon(
            "res://InesSilent/assets/ui/icon/character_icon_ines.png",
            "res://InesSilent/assets/ui/icon/character_icon_outline_ines.png",
            "res://InesSilent/scenes/ui/ines_icon.tscn",
            "res://InesSilent/assets/ui/icon/map_marker_ines.png");
        InesSkin.RegisterHands(
            "res://InesSilent/assets/ui/arm/ines_point.png",
            "res://InesSilent/assets/ui/arm/ines_rock.png",
            "res://InesSilent/assets/ui/arm/ines_paper.png",
            "res://InesSilent/assets/ui/arm/ines_scissor.png");
        InesSkin.RegisterEnergy(
            "res://InesSilent/assets/ui/energy/ines_energy_icon.png",
            "res://InesSilent/assets/ui/energy/ines_orb_layer_1.png",
            "res://InesSilent/assets/ui/energy/ines_orb_layer_2.png",
            "res://InesSilent/assets/ui/energy/ines_orb_layer_3.png",
            "res://InesSilent/assets/ui/energy/ines_orb_layer_4.png",
            "res://InesSilent/assets/ui/energy/ines_orb_layer_5.png");
        InesSkin.RegisterCardMaterial(
            "res://InesSilent/assets/ui/cards/frames/card_frame_ines.tres",
            "res://InesSilent/scenes/ui/card_trail_ines.tscn");
        Color outlineColor = new Color(0.51f, 0.0f, 0.0f, 1f);
        InesSkin.RegisterEnergyColor(outlineColor, outlineColor);
        InesSkin.RegisterPotion<PoisonPotion>(
            "res://InesSilent/assets/ui/icon/wisadele/wisadele_potion.png",
            "res://InesSilent/assets/ui/icon/wisadele/wisadele_potion_outline.png",
            "res://InesSilent/assets/ui/icon/wisadele/wisadele_potion.png");
        InesSkin.RegisterPower<PoisonPower>(
            "res://InesSilent/assets/ui/icon/wisadele/wisadele_power.png", 
            "res://InesSilent/assets/ui/icon/wisadele/wisadele_power.png");
        SkinRegistry.Register(InesSkin);
    }
}