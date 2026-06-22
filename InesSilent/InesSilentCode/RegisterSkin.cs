using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using thunninoiSkinManager.thunninoiSkinManagerCode;
using thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

namespace InesSilent.InesSilentCode;

[HarmonyPatch]
public class RegisterSkin
{
    [HarmonyPatch(typeof(SkinRegistry), nameof(SkinRegistry.SkinDbSetup))]
    [HarmonyPostfix]
    public static void RegisterInesSkin()
    {
        SkinData InesSkin = new SkinData(ModelDb.Character<Silent>().Id, "ines", "Ines")
            .RegisterCharacter(new Ines_Skin())
            .RegisterPotion(new WisadelePotion())
            .RegisterPower(new WisadelePower())
            .RegisterShivTint(new Color("d90000"));
      
        InesSkin.RegisterConfig(SkinData.SkinConfigKey.UseCardFrame, () => InesConfig.UseInesCardFrame);
        InesSkin.RegisterConfig(SkinData.SkinConfigKey.UseEnergy, () => InesConfig.UseInesEnergy);
        InesSkin.RegisterConfig(SkinData.SkinConfigKey.UseHands, () => InesConfig.UseInesMultArm);
        SkinRegistry.Register(InesSkin);
    }
}

public class Ines_Skin : CharacterSkin<Silent>
{
    public override string CombatVisual => "res://InesSilent/scenes/character/ines_default.tscn";
    public override string MerchantVisual => "res://InesSilent/scenes/character/ines_merchant.tscn";
    public override string RestVisual => "res://InesSilent/scenes/character/ines_rest_site.tscn";

    public override string CharacterSelectBg => "res://InesSilent/scenes/ui/char_select_bg_ines.tscn";
    public override string CharacterSelectPortrait => "res://InesSilent/assets/ui/characterSelect/ines_portrait.png";

    public override string CharacterIcon => "res://InesSilent/assets/ui/icon/character_icon_ines.png";
    public override string CharacterIconOutline => "res://InesSilent/assets/ui/icon/character_icon_outline_ines.png";
    public override string CharacterIconScene => "res://InesSilent/scenes/ui/ines_icon.tscn";
    public override string CharacterMapMarker => "res://InesSilent/assets/ui/icon/map_marker_ines.png";

    public override string CardFrameMaterial => "res://InesSilent/assets/ui/cards/frames/card_frame_ines.tres";
    public override string CardTrail => "res://InesSilent/scenes/ui/card_trail_ines.tscn";

    public override string EnergyIcon => "res://InesSilent/assets/ui/energy/ines_energy_icon.png";

    public override string[]? EnergyLayers =>
    [
        "res://InesSilent/assets/ui/energy/ines_orb_layer_1.png",
        "res://InesSilent/assets/ui/energy/ines_orb_layer_2.png",
        "res://InesSilent/assets/ui/energy/ines_orb_layer_3.png",
        "res://InesSilent/assets/ui/energy/ines_orb_layer_4.png",
        "res://InesSilent/assets/ui/energy/ines_orb_layer_5.png"
    ];

    public override Color? EnergyLabelColor => new Color(0.51f, 0f, 0f);
    public override Color? EnergyLabelOutlineColor => new Color(0.51f, 0f, 0f);

    public override string? HandPoint => "res://InesSilent/assets/ui/arm/ines_point.png";
    public override string? HandRock => "res://InesSilent/assets/ui/arm/ines_rock.png";
    public override string? HandPaper => "res://InesSilent/assets/ui/arm/ines_paper.png";
    public override string? HandScissors => "res://InesSilent/assets/ui/arm/ines_scissor.png";
}

public class WisadelePotion : PotionSkin<PoisonPotion>
{
    public override string? CustomSpritePath => "res://InesSilent/assets/ui/icon/wisadele/wisadele_potion.png";
    public override string? CustomSpriteOutlinePath => "res://InesSilent/assets/ui/icon/wisadele/wisadele_potion_outline.png";
    public override string? CustomThrownSpritePath => "res://InesSilent/assets/ui/icon/wisadele/wisadele_potion.png";
}

public class WisadelePower : PowerSkin<PoisonPower>
{
    public override string? CustomIconPath => "res://InesSilent/assets/ui/icon/wisadele/wisadele_power.png";
    public override string? CustomBigIconPath => "res://InesSilent/assets/ui/icon/wisadele/wisadele_power.png";
}