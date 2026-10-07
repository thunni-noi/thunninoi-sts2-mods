using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using thunninoiSkinManager.thunninoiSkinManagerCode;
using thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

namespace MakotoNecrobinder.MakotoNecrobinderCode;

public class SEESmakoto : CharacterSkin<Necrobinder>
{
    public override string CombatVisual => "res://MakotoNecrobinder/assets/sees/character_visuals/makoto.tscn";
    public override string MerchantVisual => "res://MakotoNecrobinder/assets/sees/character_visuals/makoto_merchant.tscn";
    public override string RestVisual => "res://MakotoNecrobinder/assets/sees/character_visuals/makoto_rest_site.tscn";

    public override string CharacterSelectBg => "res://MakotoNecrobinder/assets/sees/char_select/char_select_bg_makoto.tscn";
    public override string CharacterSelectPortrait => "res://MakotoNecrobinder/assets/sees/char_select/char_select_makoto.png";

    public override string CharacterIcon => "res://MakotoNecrobinder/assets/sees/ui/top_panel/makoto_icon.png";
    public override string CharacterIconOutline => "res://MakotoNecrobinder/assets/sees/ui/top_panel/makoto_icon_outline.png";
    public override string CharacterIconScene => "res://MakotoNecrobinder/assets/sees/ui/makoto_icon.tscn";
    public override string CharacterMapMarker => "res://MakotoNecrobinder/assets/sees/ui/map_marker_makoto.png";

    public override string CardFrameMaterial => "res://MakotoNecrobinder/assets/cards/frames/card_makoto.tres";
    public override string CardTrail => "res://MakotoNecrobinder/assets/sees/ui/card_trail_makoto.tscn";

    public override string EnergyIcon => "res://MakotoNecrobinder/assets/sees/ui/energy_counter/makoto_energy.png";

    /*public override string[]? EnergyLayers =>
    [
        "res://MakotoNecrobinder/assets/sees/ui/energy_counter/makoto_orb_layer_1.png",
        "res://MakotoNecrobinder/assets/sees/ui/energy_counter/makoto_orb_layer_2.png",
        "res://MakotoNecrobinder/assets/sees/ui/energy_counter/makoto_orb_layer_3.png"
    ];*/

    public override Color? EnergyLabelColor => new Color("1224CC");
    public override Color? EnergyLabelOutlineColor => new Color("1224CC");

    public override string? HandPoint => "res://MakotoNecrobinder/assets/sees/ui/hands/point.png";
    public override string? HandRock => "res://MakotoNecrobinder/assets/sees/ui/hands/rock.png";
    public override string? HandPaper => "res://MakotoNecrobinder/assets/sees/ui/hands/paper.png";
    public override string? HandScissors => "res://MakotoNecrobinder/assets/sees/ui/hands/scissors.png";
}

[HarmonyPatch(typeof(SkinRegistry), nameof(SkinRegistry.SkinDbSetup))]
public static class SkinRegister
{
    [HarmonyPostfix]
    private static void RegisterSkin()
    {
        SkinData seesMakoto = new SkinData(ModelDb.Character<Necrobinder>().Id, "seesmakoto", "SEES Makoto");
        
        seesMakoto.RegisterCharacter(new SEESmakoto());
        seesMakoto.RegisterConfig(SkinData.SkinConfigKey.UseEnergy, () => MktConfig.UseMakotoEnergy);
        seesMakoto.RegisterConfig(SkinData.SkinConfigKey.UseCardFrame, () => MktConfig.UseMakotoCardFrame);
        seesMakoto.RegisterConfig(SkinData.SkinConfigKey.UseHands, () => MktConfig.UseMakotoArms);
        
        SkinRegistry.Register(seesMakoto);
    }
}