using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;

namespace MakotoNecrobinder.MakotoNecrobinderCode.Patches;

public class VfxHelper
{
    public static void PlayOnCreature(Creature target, string vfxPath)
    {
        if (target.IsDead) return;

        var targetNode = target.GetCreatureNode();
        var vfxContainer = target.GetVfxContainer();
        if (targetNode == null || vfxContainer == null) return;
        
        PackedScene? vfxScene = PreloadManager.Cache.GetScene(vfxPath);
        var vfx = vfxScene.Instantiate<Node2D>();
        vfxContainer.AddChild(vfx);
        vfx.GlobalPosition = targetNode.VfxSpawnPosition;
    }

    public static void PlayPersonaVfx(Creature target)
    {
        if (target.IsDead) return;
        
        var creatureNode =  target.GetCreatureNode();
        var vfxContainer = target.GetVfxContainer();
        if (creatureNode == null || vfxContainer == null)
        {
            Mkt_init.Logger.Info("container not found!");
            return;
        }

        var marker = creatureNode.FindChild("summonVfx", recursive: true, owned: false) as Marker2D;
        if (marker == null)
        {
            Mkt_init.Logger.Info("Not found!");
            return;
        }
        
        Mkt_init.Logger.Info(creatureNode.GetZIndex().ToString());
        PackedScene? vfxScene = PreloadManager.Cache.GetScene("res://MakotoNecrobinder/assets/sees/vfx/casts/cast_vfx.tscn");
        var vfx = vfxScene.Instantiate<Node2D>();
        vfxContainer.AddChild(vfx);
        vfx.GlobalPosition = marker.GlobalPosition;
    }
}

[HarmonyPatch(typeof(VfxCmd), nameof(VfxCmd.PlayOnCreatureCenter))]
public class VfxReplace
{
    private const string personaVfx = "res://MakotoNecrobinder/assets/sees/vfx/heal_persona/vfx_summon_persona.tscn";
    
    [HarmonyPrefix]
    private static bool ostyHealReplace(Creature target, string path)
    {
        if (path != "vfx/vfx_heal_osty") return true;
        VfxHelper.PlayOnCreature(target, personaVfx);
        return false;
    }
}