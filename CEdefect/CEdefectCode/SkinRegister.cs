using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;
using thunninoiSkinManager.thunninoiSkinManagerCode;

namespace CEdefect.CEdefectCode.Patches;

[HarmonyPatch(typeof(SkinRegistry), nameof(SkinRegistry.SkinDbSetup))]
public class SkinRegister
{
    [HarmonyPostfix]
    private static void RegisterSkin()
    {
        SkinData ceDefaultSkin = new SkinData(ModelDb.Character<Defect>().Id, "ceterna", "Civilight Eterna");
        ceDefaultSkin.RegisterCharacter(new CE_skins());
        
        // ---------------------------------------------------
        
        SkinData epoqueSkin = new SkinData(ModelDb.Character<Defect>().Id, "ceterna2", "Condolences");
        epoqueSkin.RegisterCharacter(new CE_epoqueSkin());
        
        AddSharedResouce(ceDefaultSkin);
        AddSharedResouce(epoqueSkin);
        SkinRegistry.Register(ceDefaultSkin);
        SkinRegistry.Register(epoqueSkin);
    }

    private static void AddSharedResouce(SkinData toAdded)
    {
        toAdded.RegisterOrb(new AmiyaOrb());
        toAdded.RegisterOrb(new GoldenGlowOrb());
        toAdded.RegisterOrb(new HoshiOrb());
        toAdded.RegisterOrb(new LinOrb());
        toAdded.RegisterOrb(new LogosOrb());

        toAdded.RegisterRelic(new TheresaDoll());
        toAdded.RegisterRelic(new AmiyaDoll());

        //toAdded.RegisterSfx("charIntro",Path.Combine(CE_Init.modDirectory, "CEaudio", "deploy_sfx.wav"), 0.8f);
        //toAdded.RegisterSfx("event:/sfx/characters/defect/defect_select")
        toAdded.RegisterConfig(SkinData.SkinConfigKey.UseCardFrame, () => CE_Config.UseCivilightCardFrame);
        toAdded.RegisterConfig(SkinData.SkinConfigKey.UseEnergy, () => CE_Config.UseCivilightEnergy);
        toAdded.RegisterConfig(SkinData.SkinConfigKey.UseHands, () => CE_Config.UseCivilightEnergy);
        toAdded.RegisterConfig(SkinData.SkinConfigKey.UseDefectOrbs, () => CE_Config.UseCivilightOrbs);
    }
}