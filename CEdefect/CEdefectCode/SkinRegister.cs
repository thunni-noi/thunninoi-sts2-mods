using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;
using thunninoiSkinManager.thunninoiSkinManagerCode;

namespace CEdefect.CEdefectCode.Patches;

[HarmonyPatch]
public class SkinRegister
{
    [HarmonyPatch(typeof(SkinRegistry), nameof(SkinRegistry.SkinDbSetup))]
    [HarmonyPostfix]
    private static void RegisterSkin()
    {
        SkinData ceDefaultSkin = new SkinData("defect", "ceterna", "Civilight Eterna");
        ceDefaultSkin.RegisterVisuals("res://CEdefect/scenes/default/ce_combat.tscn",
        "res://CEdefect/scenes/default/ce_merchant.tscn",
          "res://CEdefect/scenes/default/ce_rest.tscn");
        
        ceDefaultSkin.RegisterCharacterSelect(
                  "res://CEdefect/scenes/default/ce_default_charsel.tscn",
                "res://CEdefect/assets/shared/ce_portrait.png");
        
        ceDefaultSkin.RegisterIcon("res://CEdefect/assets/default/ce_default_icon.png",
            "res://CEdefect/assets/default/ce_default_icon_outline.png",
             "res://CEdefect/scenes/default/ce_default_icon.tscn",
            "res://CEdefect/assets/shared/ce_map_icon.png");

        ceDefaultSkin.RegisterHands(
                  "res://CEdefect/assets/default/arms/ce_default_point.png",
                  "res://CEdefect/assets/default/arms/ce_default_rock.png", 
                 "res://CEdefect/assets/default/arms/ce_default_paper.png",
                "res://CEdefect/assets/default/arms/ce_default_scissor.png");
        
        // ---------------------------------------------------
        
        SkinData epoqueSkin = new SkinData("defect", "ceterna2", "Condolences");
        epoqueSkin.RegisterVisuals(
            "res://CEdefect/scenes/epoque/ce_epoque_combat.tscn",
            "res://CEdefect/scenes/epoque/ce_epoque_merchant.tscn",
            "res://CEdefect/scenes/epoque/ce_epoque_rest.tscn"
        );

        epoqueSkin.RegisterCharacterSelect(
            "res://CEdefect/scenes/epoque/charselect/ce_epoque_charsel.tscn",
            "res://CEdefect/assets/shared/ce_portrait.png"
        );

        epoqueSkin.RegisterIcon(
            "res://CEdefect/assets/epoque/ce_epoque_icon.png",
            "res://CEdefect/assets/epoque/ce_epoque_icon_outline.png",
            "res://CEdefect/scenes/epoque/ce_epoque_icon.tscn",
            "res://CEdefect/assets/shared/ce_map_icon.png"
        );

        epoqueSkin.RegisterHands(
            "res://CEdefect/assets/epoque/arms/ce_epoque_point.png",
            "res://CEdefect/assets/epoque/arms/ce_epoque_rock.png",
            "res://CEdefect/assets/epoque/arms/ce_epoque_paper.png",
            "res://CEdefect/assets/epoque/arms/ce_epoque_scissor.png"
        );
        
        AddSharedResouce(ceDefaultSkin);
        AddSharedResouce(epoqueSkin);
        SkinRegistry.Register(ceDefaultSkin);
        SkinRegistry.Register(epoqueSkin);
    }

    private static void AddSharedResouce(SkinData toAdded)
    {
        toAdded.RegisterEnergy("res://CEdefect/assets/shared/energy_counter/ce_orb_full.png",
            "res://CEdefect/assets/shared/energy_counter/ce_orb_layer_1.png",
            "res://CEdefect/assets/shared/energy_counter/ce_orb_layer_2.png",
            "res://CEdefect/assets/shared/energy_counter/ce_orb_layer_3.png",
            "res://CEdefect/assets/shared/energy_counter/ce_orb_layer_4.png",
            "res://CEdefect/assets/shared/energy_counter/ce_orb_layer_5.png");
        
        toAdded.RegisterCardMaterial(
                    "res://CEdefect/assets/shared/cards/frames/card_frame_ce.tres",
            "res://CEdefect/scenes/shared/card_trail_ce.tscn");

        toAdded.RegisterOrb("lightning_orb",
            "res://CEdefect/scenes/shared/orbs/gdglow_orb.tscn",
            "res://CEdefect/assets/shared/orbs/gdglow_orb/gdglow_icon.png",
            new Color("454545"));
        
        toAdded.RegisterOrb("dark_orb",
            "res://CEdefect/scenes/shared/orbs/logos_orb.tscn",
            "res://CEdefect/assets/shared/orbs/logos_orb/logos_icon.png",
            new Color("454545"));
        
        toAdded.RegisterOrb("frost_orb",
            "res://CEdefect/scenes/shared/orbs/hoshi_orb.tscn",
            "res://CEdefect/assets/shared/orbs/hoshi_orb/hoshi_icon.png",
            new Color("454545"));
        
        toAdded.RegisterOrb("plasma_orb",
            "res://CEdefect/scenes/shared/orbs/amiya_orb.tscn",
            "res://CEdefect/assets/shared/orbs/amiya_orb/amiya_icon.png",
            new Color("454545"));
        
        toAdded.RegisterOrb("glass_orb",
            "res://CEdefect/scenes/shared/orbs/lin_orb.tscn",
            "res://CEdefect/assets/shared/orbs/lin_orb/lin_icon.png",
            new Color("454545"));

        toAdded.RegisterEnergyColor(new Color("000000"));

        toAdded.RegisterRelic<CrackedCore>("res://CEdefect/assets/shared/relics/theresa_dolls.png",
            "res://CEdefect/assets/shared/relics/theresa_outline.png", 
            "res://CEdefect/assets/shared/relics/theresa_dolls.png");

        toAdded.RegisterRelic<InfusedCore>("res://CEdefect/assets/shared/relics/theresa_amiya_dolls.png",
            "res://CEdefect/assets/shared/relics/theresa_amiya_outline.png",
            "res://CEdefect/assets/shared/relics/theresa_amiya_dolls.png");

        toAdded.RegisterSfx("charIntro",Path.Combine(CE_Init.modDirectory, "CEaudio", "deploy_sfx.wav"), 0.8f);
        //toAdded.RegisterSfx("event:/sfx/characters/defect/defect_select")
    }
}