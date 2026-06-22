using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Potions;

namespace thunninoiSkinManager.thunninoiSkinManagerCode.Patches;

public abstract class PotionSkin
{
    public virtual ModelId? TargetPotionId => null;
    public virtual string? CustomSpritePath => null;
    public virtual string? CustomSpriteOutlinePath => null;
    public virtual string? CustomThrownSpritePath => null;
}

public abstract class PotionSkin<T> : PotionSkin where T : PotionModel
{
    public override ModelId TargetPotionId => ModelDb.GetId<T>();
}

[HarmonyPatch]
class PotionPatch
{
    private static readonly FieldInfo PotionModel = AccessTools.Field(typeof(NPotion), "_model");
    
    [HarmonyPatch(typeof(NPotion), "Reload")]
    [HarmonyPostfix]
    public static void PotionReload(NPotion __instance)
    {
        if (PotionModel.GetValue(__instance) is not PotionModel model) return;
        if (!__instance.IsNodeReady()) return;
        PotionSkin? skinData = SkinRegistry.ResolvePotion(model.Id);
        if (skinData == null) return;
        
        if (skinData.CustomSpritePath != null) __instance.Image.Texture = PreloadManager.Cache.GetTexture2D(skinData.CustomSpritePath);
        if (skinData.CustomSpriteOutlinePath != null) __instance.Outline.Texture = PreloadManager.Cache.GetTexture2D(skinData.CustomSpriteOutlinePath);
        
    }

    [HarmonyPatch(typeof(PotionModel), "get_Image")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    public static bool PotionThrownImage(PotionModel __instance, ref Texture2D? __result)
    {
        PotionSkin? skinData = SkinRegistry.ResolvePotion(__instance.Id);
        if (skinData is PotionSkin)
        {
            if (skinData.CustomThrownSpritePath != null)
            {
                __result = PreloadManager.Cache.GetTexture2D(skinData.CustomThrownSpritePath);
                return false;
            }
        }
        return true;
    }
}
    
    