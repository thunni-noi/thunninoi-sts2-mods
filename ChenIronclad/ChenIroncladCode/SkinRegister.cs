using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using thunninoiSkinManager.thunninoiSkinManagerCode;
using thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

namespace ChenIronclad.ChenIroncladCode;

[HarmonyPatch(typeof(SkinRegistry), nameof(SkinRegistry.SkinDbSetup))]
public class SkinRegister
{   
    [HarmonyPostfix]
    private static void RegisterSkin()
    {
        ChenIroncladInit.Logger.Info("Registering skin");
        SkinData chenSkin = new SkinData(ModelDb.Character<Ironclad>().Id, "chen", "Dawnstreak");
        chenSkin.RegisterCharacter(new Chen_Skin());
        chenSkin.RegisterConfig(SkinData.SkinConfigKey.UseCardFrame, () => ChenConfig.UseChenCardFrame);
        chenSkin.RegisterConfig(SkinData.SkinConfigKey.UseEnergy, () => ChenConfig.UseChenEnergy);
        chenSkin.RegisterConfig(SkinData.SkinConfigKey.UseHands, () => ChenConfig.UseChenMultArm);
        SkinRegistry.Register(chenSkin);

    }
}

public class Chen_Skin : CharacterSkin<Ironclad>
{
    public override string CombatVisual => "res://ChenIronclad/scenes/character/chen_dawnstreak.tscn";
    public override string MerchantVisual => "res://ChenIronclad/scenes/character/chen_dawnstreak_merchant.tscn";
    public override string RestVisual => "res://ChenIronclad/scenes/character/chen_dawnstreak_rest_site.tscn";

    public override string CharacterSelectBg => "res://ChenIronclad/scenes/ui/chen_charselect_bg.tscn";
    public override string CharacterSelectPortrait => "res://ChenIronclad/assets/ui/characterSelect/chen_portrait.png";
    public override string CharacterSelectTransition => "res://materials/transitions/silent_transition_mat.tres";

    public override string CharacterIcon => "res://ChenIronclad/assets/ui/icon/character_icon_chen.png";
    public override string CharacterIconOutline => "res://ChenIronclad/assets/ui/icon/character_icon_outline_chen.png";
    public override string CharacterIconScene => "res://ChenIronclad/scenes/ui/chen_icon.tscn";
    public override string CharacterMapMarker => "res://ChenIronclad/assets/ui/icon/map_icon_chen.png";

    public override string CardFrameMaterial => "res://ChenIronclad/assets/ui/cards/frames/card_frame_chen.tres";
    public override string CardTrail => "res://ChenIronclad/scenes/ui/chen_card_trail.tscn";

    public override string EnergyIcon => "res://ChenIronclad/assets/ui/energy/chen_energy_icon.png";

    public override string[]? EnergyLayers =>
    [
        "res://ChenIronclad/assets/ui/energy/chen_orb_layer_1.png",
        "res://ChenIronclad/assets/ui/energy/chen_orb_layer_2.png",
        "res://ChenIronclad/assets/ui/energy/chen_orb_layer_3.png",
        "res://ChenIronclad/assets/ui/energy/chen_orb_layer_4.png",
        "res://ChenIronclad/assets/ui/energy/chen_orb_layer_5.png"
    ];

    public override Color? EnergyLabelColor => new Color("0000AA");
    public override Color? EnergyLabelOutlineColor => new Color("0000AA");

    public override string? HandPoint => "res://ChenIronclad/assets/ui/arm/chen_pointing.png";
    public override string? HandRock => "res://ChenIronclad/assets/ui/arm/chen_rock.png";
    public override string? HandPaper => "res://ChenIronclad/assets/ui/arm/chen_paper.png";
    public override string? HandScissors => "res://ChenIronclad/assets/ui/arm/chen_scissor.png";
}