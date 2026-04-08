using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;
using MegaCrit.Sts2.Core.Random;

namespace InesSilent.InesSilentCode.core;

[GlobalClass]
public partial class InesMerchantCharacter : NMerchantCharacter
{
    public override void _Ready()
    {
        MegaTrackEntry megaTrackEntry = new MegaSprite(GetChild(0)).GetAnimationState().SetAnimation("relaxed_loop", true);
        if (true)
        {
            megaTrackEntry?.SetTrackTime(megaTrackEntry.GetAnimationEnd() * Rng.Chaotic.NextFloat());
        }
    }
}