using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;
using MegaCrit.Sts2.Core.Random;

namespace ChenIronclad.ChenIroncladCode.core;

[GlobalClass]
public partial class ChenMerchantCharacter : NMerchantCharacter
{
    // force play animation because for unknown reason it didn't work
    public override void _Ready()
    {
        MegaTrackEntry megaTrackEntry = new MegaSprite(GetChild(0)).GetAnimationState().SetAnimation("relaxed_loop", true);
        if (true)
        {
            megaTrackEntry?.SetTrackTime(megaTrackEntry.GetAnimationEnd() * Rng.Chaotic.NextFloat());
        }
    }
}