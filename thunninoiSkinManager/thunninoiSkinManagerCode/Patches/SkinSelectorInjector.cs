using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

namespace thunninoiSkinManager.thunninoiSkinManagerCode;

[HarmonyPatch(typeof(NCharacterSelectScreen), "SelectCharacter")]
public class SkinSelectorInjector
{
    private const String scenePath = "res://thunninoiSkinManager/SkinSelector.tscn";
    
    [HarmonyPostfix]
    private static void Postfix(NCharacterSelectScreen __instance, NCharacterSelectButton charSelectButton)
    {
        Node? existedSkinSelector = __instance.GetNodeOrNull("SkinSelector");
        if (existedSkinSelector != null)
        {
            __instance.RemoveChildSafely(existedSkinSelector);
            existedSkinSelector.QueueFree();
        }

        if (charSelectButton.IsRandom) return;
        
        CharacterModel selectedChar = charSelectButton.Character;
        // Control bgContainer = __instance.GetNode<Control>("AnimatedBg");
        PackedScene scene = ResourceLoader.Load<PackedScene>(scenePath);
        SkinSelector skinSelector = scene.Instantiate<SkinSelector>();

        skinSelector.Position = new Vector2(910, 710);
        //skinSelector.Scale = new Vector2(0.7f, 0.7f);
        skinSelector.characterModel = selectedChar;
        skinSelector.characterId = selectedChar.Id;
        

        __instance.AddChildSafely(skinSelector);
        
        // portrait button
    }
}