using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

namespace thunninoiSkinManager.thunninoiSkinManagerCode;

[HarmonyPatch]
public class SkinSelectorInjector
{
    //You're recommended but not required to keep all your code in this package and all your assets in the thunninoiSkinManager folder.
    private const String scenePath = "res://thunninoiSkinManager/SkinSelector.tscn";

    [HarmonyPatch(typeof(NCharacterSelectScreen), "SelectCharacter")]
    [HarmonyPostfix]
    private static void SelectCharacterPostfix(NCharacterSelectScreen __instance, CharacterModel characterModel)
    {
        Node? existedSkinSelector = __instance.GetNodeOrNull("SkinSelector");
        __instance.RemoveChildSafely(existedSkinSelector);

        if (characterModel.Id.Entry.ToLower() == "random_character") return;
        Control bgContainer = __instance.GetNode<Control>("AnimatedBg");
        modEntry.Logger.Info(characterModel.ToString());
        foreach (Node child in bgContainer.GetChildren())
        {
            modEntry.Logger.Info(child.Name.ToString());
        }
        PackedScene scene = ResourceLoader.Load<PackedScene>(scenePath);
        SkinSelector skinSelector = scene.Instantiate<SkinSelector>();

        skinSelector.Position = new Vector2(910, 710);
        //skinSelector.Scale = new Vector2(0.7f, 0.7f);
        skinSelector.characterModel = characterModel;
        skinSelector.characterId = characterModel.Id.Entry.ToLower();
        

        __instance.AddChildSafely(skinSelector);
        
        // portrait button
        HBoxContainer? portraitRow =  __instance.GetNode<HBoxContainer>("CharSelectButtons/ButtonContainer/DEFECT_button/PlayerIconContainer");
        foreach (Node child in portraitRow.GetChildren())
        {
            modEntry.Logger.Info("Button: " + child.Name + "-->" + child.GetType());
        }
        
    }
}