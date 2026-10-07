using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using thunninoiSkinManager;
using thunninoiSkinManager.thunninoiSkinManagerCode;

namespace MakotoNecrobinder.MakotoNecrobinderCode.Patches;

[HarmonyPatch(typeof(NEnergyCounter), "_Ready")]
public static class MakotoEnergy
{
    private static readonly AccessTools.FieldRef<NEnergyCounter, Player> PlayerField = AccessTools.FieldRefAccess<NEnergyCounter, Player>("_player");
    [HarmonyPostfix]
    private static void EnergyCounterReplace(NEnergyCounter __instance)
    {
        Player? player = PlayerField.Invoke(__instance);
        if (!SkinRegistry.IsUsingSkin(ModelDb.Character<Necrobinder>().Id, "seesmakoto")) return;
        if (!MktConfig.UseMakotoEnergy) return;
        if (player.Character.Id != ModelDb.Character<Necrobinder>().Id) return;
        //ModelId? charId = player.Character.Id;
        //if (!SkinRegistry.ResolveConfig(charId, SkinData.SkinConfigKey.UseEnergy)) return;
        //if (SkinRegistry.GetActiveSkin(charId).IsDefault) return;
        modEntry.Logger.Info("EnergyCounterReplace - Trying to set layers");
        SetEnergyLayer(__instance, "Layers/Layer1", "res://MakotoNecrobinder/assets/sees/ui/energy_counter/makoto_orb_layer_1.png");
        SetEnergyLayer(__instance, "Layers/RotationLayers/Layer2", "res://MakotoNecrobinder/assets/sees/ui/energy_counter/makoto_orb_layer_2.png");
        SetEnergyLayer(__instance, "Layers/Layer3", "res://MakotoNecrobinder/assets/sees/ui/energy_counter/makoto_orb_layer_3.png");
    }
    
    private static void SetEnergyLayer(NEnergyCounter counter, string nodePath, string texturePath)
    {
        Texture2D? newTexture = PreloadManager.Cache.GetTexture2D(texturePath);
        TextureRect? textureNode = counter.GetNodeOrNull<TextureRect>(nodePath);
        if (textureNode == null) return;
        textureNode.Texture = newTexture;
    }
}