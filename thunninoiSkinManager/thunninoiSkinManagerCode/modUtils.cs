using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace thunninoiSkinManager.thunninoiSkinManagerCode.Core;

public class modUtils
{
    public static bool IsOrbEnabled()
    {
        return SkinRegistry.ResolveConfig(ModelDb.Character<Defect>().Id, SkinData.SkinConfigKey.UseDefectOrbs);
    }
    
    public static bool ArmReplace(CharacterModel __instance, Func<SkinData, string?> selector, ref string __result)
    {
        ModelId charId = __instance.Id;
        if (!SkinRegistry.ResolveConfig(charId, SkinData.SkinConfigKey.UseHands)) return true;
        string? armPath = SkinRegistry.ResolvePath(charId, selector);
        if (string.IsNullOrWhiteSpace(armPath)) return true;
        __result = armPath;
        return false;
    }
    
    public static void SetEnergyLayer(NEnergyCounter counter, string nodePath, Texture2D? newTexture)
    {
        if (newTexture == null) return;
        TextureRect? textureNode = counter.GetNodeOrNull<TextureRect>(nodePath);
        if (textureNode == null) return;
        textureNode.Texture = newTexture;
    }
}