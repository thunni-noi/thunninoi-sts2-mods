using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;
using MegaCrit.Sts2.Core.Random;

namespace CEdefect.CEdefectCode.Core;

[GlobalClass]
public partial class CE_MerchantVisuals : NMerchantCharacter
{
    public override void _Ready()
    {
        MegaTrackEntry megaTrackEntry = new MegaSprite(GetChild(0)).GetAnimationState().SetAnimation("relaxed_loop", true);
        megaTrackEntry?.SetTrackTime(megaTrackEntry.GetAnimationEnd() * Rng.Chaotic.NextFloat());
    }
}