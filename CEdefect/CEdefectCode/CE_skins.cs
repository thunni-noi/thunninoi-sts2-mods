using Godot;
using MegaCrit.Sts2.Core.Models.Characters;
using thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

namespace CEdefect.CEdefectCode;

public class CE_skins : CharacterSkin<Defect>
{
    public override string CombatVisual => "res://CEdefect/scenes/default/ce_combat.tscn";
    public override string MerchantVisual => "res://CEdefect/scenes/default/ce_merchant.tscn";
    public override string RestVisual => "res://CEdefect/scenes/default/ce_rest.tscn";

    public override string CharacterSelectBg => "res://CEdefect/scenes/default/ce_default_charsel.tscn";
    public override string CharacterSelectPortrait => "res://CEdefect/assets/shared/ce_portrait.png";

    public override string CharacterIcon => "res://CEdefect/assets/default/ce_default_icon.png";
    public override string CharacterIconOutline => "res://CEdefect/assets/default/ce_default_icon_outline.png";
    public override string CharacterIconScene => "res://CEdefect/scenes/default/ce_default_icon.tscn";
    public override string CharacterMapMarker => "res://CEdefect/assets/shared/ce_map_icon.png";

    public override string CardFrameMaterial => "res://CEdefect/assets/shared/cards/frames/card_frame_ce.tres";
    public override string CardTrail => "res://CEdefect/scenes/shared/card_trail_ce.tscn";

    public override string EnergyIcon => "res://CEdefect/assets/shared/energy_counter/ce_orb_full.png";

    public override string[]? EnergyLayers =>
    [
        "res://CEdefect/assets/shared/energy_counter/ce_orb_layer_1.png",
        "res://CEdefect/assets/shared/energy_counter/ce_orb_layer_2.png",
        "res://CEdefect/assets/shared/energy_counter/ce_orb_layer_3.png",
        "res://CEdefect/assets/shared/energy_counter/ce_orb_layer_4.png",
        "res://CEdefect/assets/shared/energy_counter/ce_orb_layer_5.png"
    ];

    public override Color? EnergyLabelColor => new Color("000000");
    public override Color? EnergyLabelOutlineColor => new Color("000000");

    public override string? HandPoint => "res://CEdefect/assets/default/arms/ce_default_point.png";
    public override string? HandRock => "res://CEdefect/assets/default/arms/ce_default_rock.png";
    public override string? HandPaper => "res://CEdefect/assets/default/arms/ce_default_paper.png";
    public override string? HandScissors => "res://CEdefect/assets/default/arms/ce_default_scissor.png";
}

public class CE_epoqueSkin : CharacterSkin<Defect>
{
    public override string CombatVisual => "res://CEdefect/scenes/epoque/ce_epoque_combat.tscn";
    public override string MerchantVisual => "res://CEdefect/scenes/epoque/ce_epoque_merchant.tscn";
    public override string RestVisual => "res://CEdefect/scenes/epoque/ce_epoque_rest.tscn";

    public override string CharacterSelectBg => "res://CEdefect/scenes/epoque/charselect/ce_epoque_charsel.tscn";
    public override string CharacterSelectPortrait => "res://CEdefect/assets/shared/ce_portrait.png";

    public override string CharacterIcon =>  "res://CEdefect/assets/epoque/ce_epoque_icon.png";
    public override string CharacterIconOutline => "res://CEdefect/assets/epoque/ce_epoque_icon_outline.png";
    public override string CharacterIconScene => "res://CEdefect/scenes/epoque/ce_epoque_icon.tscn";
    public override string CharacterMapMarker => "res://CEdefect/assets/shared/ce_map_icon.png";

    public override string CardFrameMaterial => "res://CEdefect/assets/shared/cards/frames/card_frame_ce.tres";
    public override string CardTrail => "res://CEdefect/scenes/shared/card_trail_ce.tscn";

    public override string EnergyIcon => "res://CEdefect/assets/shared/energy_counter/ce_orb_full.png";

    public override string[]? EnergyLayers =>
    [
        "res://CEdefect/assets/shared/energy_counter/ce_orb_layer_1.png",
        "res://CEdefect/assets/shared/energy_counter/ce_orb_layer_2.png",
        "res://CEdefect/assets/shared/energy_counter/ce_orb_layer_3.png",
        "res://CEdefect/assets/shared/energy_counter/ce_orb_layer_4.png",
        "res://CEdefect/assets/shared/energy_counter/ce_orb_layer_5.png"
    ];

    public override Color? EnergyLabelColor => new Color("000000");
    public override Color? EnergyLabelOutlineColor => new Color("000000");

    public override string? HandPoint =>  "res://CEdefect/assets/epoque/arms/ce_epoque_point.png";
    public override string? HandRock => "res://CEdefect/assets/epoque/arms/ce_epoque_rock.png";
    public override string? HandPaper => "res://CEdefect/assets/epoque/arms/ce_epoque_paper.png";
    public override string? HandScissors => "res://CEdefect/assets/epoque/arms/ce_epoque_scissor.png";
}