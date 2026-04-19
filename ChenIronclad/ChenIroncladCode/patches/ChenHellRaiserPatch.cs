using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;

namespace ChenIronclad.ChenIroncladCode.patches;

[HarmonyPatch]
public class ChenHellRaiserPatch
{
    private const string ChenBladePath = "res://ChenIronclad/assets/character/chen_blade.png";
    
    [HarmonyPatch(typeof(NHellraiserSwordVfx), nameof(NHellraiserSwordVfx._Ready))]
    [HarmonyPostfix]
    public static void Chen_SwordVfxPatch(NHellraiserSwordVfx __instance)
    {
        if (ChenConfig.DebugMode) Log.Info($"[CHEN] Patching HellraiserSwordVfx from {__instance.Name}");
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
    public static void Chen_AttackVfxPatch(NHellraiserAttackVfx __instance)
    {
        if (ChenConfig.DebugMode) Log.Info($"[CHEN] Patching HellraiserAttackVfx from {__instance.Name}");
        if (!ChenConfig.UseChixiaoHellraiser) return; 
        var node = __instance.GetNodeOrNull<TextureRect>("%Sword");
        if (node != null)
        {
            node.Texture = ResourceLoader.Load<Texture2D>(ChenBladePath);
        }
    }
}