using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;

namespace ChenIronclad.ChenIroncladCode.patches;

[HarmonyPatch]
public class HellRaiserPatch
{
    private const string ChenBladePath = "res://ChenIronclad/assets/character/chen_blade.png";
    
    [HarmonyPatch(typeof(NHellraiserSwordVfx), nameof(NHellraiserSwordVfx._Ready))]
    [HarmonyPostfix]
    public static void SwordVfxPatch(NHellraiserSwordVfx __instance)
    {
        if (!ChenConfig.UseChixiaoHellraiser) return; 
        var swordField = typeof(NHellraiserSwordVfx)
            .GetField("_sword", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (swordField?.GetValue(__instance) is TextureRect swordRect)
        {
            swordRect.Texture = ResourceLoader.Load<Texture2D>(ChenBladePath);
        }
    }
    
    [HarmonyPatch(typeof(NHellraiserAttackVfx), nameof(NHellraiserAttackVfx._Ready))]
    [HarmonyPostfix]
    public static void AttackVfxPatch(NHellraiserAttackVfx __instance)
    {
        if (!ChenConfig.UseChixiaoHellraiser) return; 
        var node = __instance.GetNodeOrNull<TextureRect>("%Sword");
        if (node != null)
        {
            node.Texture = ResourceLoader.Load<Texture2D>(ChenBladePath);
        }
    }
}