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
using SkinRegistry = thunninoiSkinManager.thunninoiSkinManagerCode.SkinRegistry;

namespace CEdefect.CEdefectCode.Patches;

[HarmonyPatch]
public class CE_OrbPatches
{
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
        if (!CE_Utils.isUsingSkin()) return true;
        if (CE_Config.UseCeCompatMode) return true;
        if (!CE_Config.UseCivilightOrbs) return true;
        if (!target.IsEnemy) return true;
        //Log.Info("[CE] Is enemy" + target.);
        if (!string.Equals(path, "vfx/vfx_attack_lightning", StringComparison.Ordinal) || (target.IsPlayer)) return true;
        PlayGoldenGlowVfx(target);
        return false;
    }
}