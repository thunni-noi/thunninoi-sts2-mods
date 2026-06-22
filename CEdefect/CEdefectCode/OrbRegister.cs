using System.Reflection.Emit;
using Godot;
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Nodes.Orbs;
using thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

namespace CEdefect.CEdefectCode;

public class AmiyaOrb : OrbSkin<PlasmaOrb>
{
    public override string? CustomIconPath => "res://CEdefect/assets/shared/orbs/amiya_orb/amiya_icon.png";
    public override string? CustomSpritePath => "res://CEdefect/scenes/shared/orbs/amiya_orb.tscn";
    public override Color? CustomDarkenedColor => new Color("454545");
}
public class GoldenGlowOrb : OrbSkin<LightningOrb>
{
    public override string CustomIconPath => "res://CEdefect/assets/shared/orbs/gdglow_orb/gdglow_icon.png";
    public override Color? CustomDarkenedColor => new Color("454545");
    public override string? CustomSpritePath => "res://CEdefect/scenes/shared/orbs/gdglow_orb.tscn";
}

public class HoshiOrb : OrbSkin<FrostOrb>
{
    public override string CustomIconPath => "res://CEdefect/assets/shared/orbs/hoshi_orb/hoshi_icon.png";
    public override Color? CustomDarkenedColor => new Color("454545");
    public override string? CustomSpritePath => "res://CEdefect/scenes/shared/orbs/hoshi_orb.tscn";
}

public class LinOrb : OrbSkin<GlassOrb>
{
    public override string CustomIconPath => "res://CEdefect/assets/shared/orbs/lin_orb/lin_icon.png";
    public override Color? CustomDarkenedColor => new Color("454545");
    public override string? CustomSpritePath => "res://CEdefect/scenes/shared/orbs/lin_orb.tscn";
}

public class LogosOrb : OrbSkin<DarkOrb>
{
    public override string CustomIconPath => "res://CEdefect/assets/shared/orbs/logos_orb/logos_icon.png";
    public override Color? CustomDarkenedColor => new Color("454545");
    public override string? CustomSpritePath => "res://CEdefect/scenes/shared/orbs/logos_orb.tscn";
}