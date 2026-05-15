using Godot;
using HarmonyLib;
using thunninoiSkinManager.thunninoiSkinManagerCode;

namespace ChenIronclad.ChenIroncladCode;

[HarmonyPatch]
public class SkinRegister
{   
    [HarmonyPatch(typeof(SkinRegistry), nameof(SkinRegistry.SkinDbSetup))]
    [HarmonyPostfix]
    private static void RegisterSkin()
    {
        ChenIroncladInit.Logger.Info("Registering skin");
        SkinData chenSkin = new SkinData("ironclad", "chen", "Dawnstreak");
        chenSkin.RegisterVisuals(
            "res://ChenIronclad/scenes/character/chen_dawnstreak.tscn",
            "res://ChenIronclad/scenes/character/chen_dawnstreak_merchant.tscn",
            "res://ChenIronclad/scenes/character/chen_dawnstreak_rest_site.tscn");
        chenSkin.RegisterCharacterSelect(
            "res://ChenIronclad/scenes/ui/chen_charselect_bg.tscn",
            "res://ChenIronclad/assets/ui/characterSelect/chen_portrait.png",
            "res://materials/transitions/silent_transition_mat.tres");
        chenSkin.RegisterIcon(
            "res://ChenIronclad/assets/ui/icon/character_icon_chen.png",
            "res://ChenIronclad/assets/ui/icon/character_icon_outline_chen.png",
            "res://ChenIronclad/scenes/ui/chen_icon.tscn",
            "res://ChenIronclad/assets/ui/icon/map_icon_chen.png");
        chenSkin.RegisterHands(
            "res://ChenIronclad/assets/ui/arm/chen_pointing.png",
            "res://ChenIronclad/assets/ui/arm/chen_rock.png",
            "res://ChenIronclad/assets/ui/arm/chen_paper.png",
            "res://ChenIronclad/assets/ui/arm/chen_scissor.png");
        chenSkin.RegisterCardMaterial(
            "res://ChenIronclad/assets/ui/cards/frames/card_frame_chen.tres",
            "res://ChenIronclad/scenes/ui/chen_card_trail.tscn");
        chenSkin.RegisterEnergy(
            "res://ChenIronclad/assets/ui/energy/chen_energy_icon.png",
            "res://ChenIronclad/assets/ui/energy/chen_orb_layer_1.png",
            "res://ChenIronclad/assets/ui/energy/chen_orb_layer_2.png",
            "res://ChenIronclad/assets/ui/energy/chen_orb_layer_3.png",
            "res://ChenIronclad/assets/ui/energy/chen_orb_layer_4.png",
            "res://ChenIronclad/assets/ui/energy/chen_orb_layer_5.png"
        );
        chenSkin.RegisterConfig(SkinData.SkinConfigKey.UseCardFrame, () => ChenConfig.UseChenCardFrame);
        chenSkin.RegisterConfig(SkinData.SkinConfigKey.UseEnergy, () => ChenConfig.UseChenEnergy);
        chenSkin.RegisterConfig(SkinData.SkinConfigKey.UseHands, () => ChenConfig.UseChenMultArm);
        chenSkin.RegisterEnergyColor(new Color("#0000AA"), new Color("#0000AA"));
        SkinRegistry.Register(chenSkin);

    }
}