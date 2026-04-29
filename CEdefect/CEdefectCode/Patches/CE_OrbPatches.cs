using CEdefect.CEdefectCode.SkinManager;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace CEdefect.CEdefectCode.Patches;

[HarmonyPatch]
public class CE_OrbPatches
{
    [HarmonyPatch(typeof(OrbModel), nameof(OrbModel.CreateSprite))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_OrbModelPatches(OrbModel __instance, ref Node2D __result)
    {
        if (!CE_Config.UseCivilightOrbs) return true;
        string replace_path = "";
        string current_orb = __instance.Id.Entry.ToLower();
        switch (current_orb)
        {
            case "lightning_orb":
                CE_Utils.Logger("Patching lightning orb");
                replace_path = SharedResources.LightningOrb;
                break;
            case "dark_orb":
                CE_Utils.Logger("Patching dark orb");
                replace_path = SharedResources.DarkOrb;
                break;
            case "frost_orb":
                CE_Utils.Logger("Patching frost orb");
                replace_path = SharedResources.FrostOrb;
                break;
            case "plasma_orb":
                CE_Utils.Logger("Patching plasma orb");
                replace_path = SharedResources.PlasmaOrb;
                break;
            case "glass_orb":
                CE_Utils.Logger("Patching glass orb");
                replace_path = SharedResources.GlassOrb;
                break;
            default:
                CE_Utils.Logger("Unknown orb");
                break;
        }

        if (string.IsNullOrWhiteSpace(replace_path)) return true;
        
        PackedScene orbScene = ResourceLoader.Load<PackedScene>(replace_path, null, ResourceLoader.CacheMode.Reuse);
        if (orbScene == null)
        {
            CE_Utils.Logger("Could not find orb resource : " + replace_path);
            return true;
        }
        Node2D orbNode = orbScene.Instantiate<Node2D>();
        foreach (Node child in orbNode.GetChildren())
        {
            CE_Utils.Logger("Child node " + child.Name);
        }
        Node orbSprite = orbNode.GetNodeOrNull("OrbRender");
        CE_Utils.Logger("Orb Sprite " + orbSprite.Name);
        new MegaSprite(orbNode.GetNode("OrbRender/OrbSprite")).GetAnimationState().SetAnimation("idle_loop");
        __result = orbNode;
        return false;

    }

    [HarmonyPatch(typeof(OrbModel), "get_IconPath")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_OrbIconPatches(OrbModel __instance, ref string __result)
    {
        if (!CE_Config.UseCivilightOrbs) return true;
        string replace_path = "";
        string current_orb = __instance.Id.Entry.ToLower();
        switch (current_orb)
        {
            case "lightning_orb":
                CE_Utils.Logger("Patching lightning orb");
                replace_path = SharedResources.LightningOrbLogo;
                break;
            case "dark_orb":
                CE_Utils.Logger("Patching dark orb");
                replace_path = SharedResources.DarkOrbLogo;
                break;
            case "frost_orb":
                CE_Utils.Logger("Patching frost orb");
                replace_path = SharedResources.FrostOrbLogo;
                break;
            case "plasma_orb":
                CE_Utils.Logger("Patching plasma orb");
                replace_path = SharedResources.PlasmaOrbLogo;
                break;
            case "glass_orb":
                CE_Utils.Logger("Patching glass orb");
                replace_path = SharedResources.GlassOrbLogo;
                break;
            default:
                CE_Utils.Logger("Unknown orb");
                break;
        }

        if (string.IsNullOrWhiteSpace(replace_path)) return true;
        
        __result = replace_path;
        return false;
    }
    
    [HarmonyPatch(typeof(LightningOrb), "get_DarkenedColor")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_LightningOrbDarkened(OrbModel __instance, ref Color __result)
    {
        if (!CE_Config.UseCivilightOrbs) return true;
        __result = new Color("454545");
        return false;
    }
    
    [HarmonyPatch(typeof(DarkOrb), "get_DarkenedColor")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_DarkOrbDarkened(OrbModel __instance, ref Color __result)
    {
        if (!CE_Config.UseCivilightOrbs) return true;
        __result = new Color("454545");
        return false;
    }
    
    [HarmonyPatch(typeof(FrostOrb), "get_DarkenedColor")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_FrostOrbDarkened(OrbModel __instance, ref Color __result)
    {
        if (!CE_Config.UseCivilightOrbs) return true;
        __result = new Color("454545");
        return false;
    }
    
    [HarmonyPatch(typeof(PlasmaOrb), "get_DarkenedColor")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_PlasmaOrbDarkened(OrbModel __instance, ref Color __result)
    {
        if (!CE_Config.UseCivilightOrbs) return true;
        __result = new Color("454545");
        return false;
    }
    
    [HarmonyPatch(typeof(GlassOrb), "get_DarkenedColor")]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool CE_GlassOrbDarkened(OrbModel __instance, ref Color __result)
    {
        if (!CE_Config.UseCivilightOrbs) return true;
        __result = new Color("454545");
        return false;
    }
    
    // Lightning vfx
    public static void PlayGoldenGlowVfx(Creature target)
    {
        CE_Utils.Logger("Playing Goldenglow vfx");
        string vfxPath = "res://CEdefect/scenes/shared/gdglow_lightning/gdglow_lightning_vfx.tscn";
        int droneCount = 2;
        float scatterRadius = 250f;
        Random rng = new Random();

        if (NCombatRoom.Instance == null)
        {
            CE_Utils.Logger("[CE] No combat room");
            return;
        }

        NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(target);
        if (creatureNode == null)  {
            CE_Utils.Logger("No creature node");
            return;
        }

        Vector2 centerPos = creatureNode.VfxSpawnPosition;

        if (!ResourceLoader.Exists(vfxPath, ""))  {
            CE_Utils.Logger("cannot find lightning scene");
            return;
        };
        PackedScene scene = ResourceLoader.Load<PackedScene>(vfxPath, null, ResourceLoader.CacheMode.Reuse);
        if (scene == null)  {
            CE_Utils.Logger("cannot get lightning scene");
            return;
        };

        for (int i = 0; i < droneCount; i++)
        {
            float angle = (float)(rng.NextDouble() * Math.PI * 2);
            Vector2 offset = new Vector2(
                Mathf.Cos(angle) * scatterRadius,
                Mathf.Sin(angle) * scatterRadius
            );
            Vector2 spawnPos = centerPos + offset;
            
            Node2D instance = scene.Instantiate<Node2D>();

            instance.Rotation = centerPos.DirectionTo(spawnPos).Angle() + (Mathf.Pi/2);
            NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(instance);
            instance.GlobalPosition = spawnPos;
        }
    }
    
    [HarmonyPatch(typeof(VfxCmd), nameof(VfxCmd.PlayOnCreature))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.High)]
    private static bool LightningVfxPatch(Creature target, ref string path)
    {
        if (CE_Config.UseCeCompatMode) return true;
        if (!CE_Config.UseCivilightOrbs) return true;
        if (!target.IsEnemy) return true;
        //Log.Info("[CE] Is enemy" + target.);
        if (!string.Equals(path, "vfx/vfx_attack_lightning", StringComparison.Ordinal) || (target.IsPlayer)) return true;
        PlayGoldenGlowVfx(target);
        return false;
    }
}