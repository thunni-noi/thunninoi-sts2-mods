using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;

namespace thunninoiSkinManager.thunninoiSkinManagerCode;

[HarmonyPatch]
public class PreloadHelper
{
    // Added skin into pre-loading list
    [HarmonyPatch(typeof(PreloadManager), nameof(PreloadManager.LoadCommonAndMainMenuAssets))]
    [HarmonyPostfix]
    private static void AddSkinPreload()
    {
        // Collect all skin asset paths
        List<string> skinPaths = new();

        foreach (string characterId in SkinRegistry.GetAllCharacterIds())
        {
            foreach (SkinData skin in SkinRegistry.GetAllSkins(characterId))
            {
                if (skin.IsDefault) continue;
                PreloadPathsGetter(skin, skinPaths);
            }
        }

        if (skinPaths.Count == 0) return;

        modEntry.Logger.Info($"Adding {skinPaths.Count} skin assets to preload.");

        // Add to the Common asset set so the game's own loader handles them
        foreach (string path in skinPaths)
        {
            if (!string.IsNullOrWhiteSpace(path) && ResourceLoader.Exists(path, ""))
                ((HashSet<string>)AssetSets.CommonAssets).Add(path);
        }
    }

    private static void PreloadPathsGetter(SkinData skin, List<string> paths)
    {
        void Add(string? p) { if (!string.IsNullOrWhiteSpace(p)) paths.Add(p); }

        Add(skin.CombatVisuals);
        Add(skin.MerchantAnim);
        Add(skin.RestSiteAnim);
        Add(skin.CharacterSelectBg);
        Add(skin.CharacterSelectIcon);
        Add(skin.TopPanelIcon);
        Add(skin.TopPanelIconOutline);
        Add(skin.CardFrameMaterial);
        Add(skin.CardTrail);
        Add(skin.EnergyIcon);
        Add(skin.EnergyLayer1);
        Add(skin.EnergyLayer2);
        Add(skin.EnergyLayer3);
        Add(skin.MapMarker);
        Add(skin.PreviewSkeletonData);

        foreach (var potion in skin.Potions.Values)
        {
            Add(potion.SpritePath);
            Add(potion.OutlinePath);
        }

        foreach (var relic in skin.Relics.Values)
        {
            Add(relic.IconPath);
            Add(relic.OutlinePath);
            Add(relic.BigIconPath);
        }

        foreach (var orbData in skin.OrbSkins.Values)
        {
            Add(orbData.OrbIconPath);
            Add(orbData.OrbScenePath);
        }
        
    }
}