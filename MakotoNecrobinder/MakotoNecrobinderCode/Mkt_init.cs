using System.Reflection;
using BaseLib.Audio;
using BaseLib.Config;
using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace MakotoNecrobinder.MakotoNecrobinderCode;

//You're recommended but not required to keep all your code in this package and all your assets in the MakotoNecrobinder folder.
[ModInitializer(nameof(Initialize))]
public partial class Mkt_init : Node
{
    public const string ModId = "MakotoNecrobinder"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);
    

    public static void Initialize()
    {
        var assembly = Assembly.GetExecutingAssembly();
        //If you want to use scripts defined in your mod for Godot scenes, uncomment the following line.
        //Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(assembly);
        Harmony harmony = new(ModId);
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(assembly);
        ModConfigRegistry.Register(ModId, new MktConfig());
        harmony.PatchAll(assembly);
    }
    
    // Check if game is in beta or not for compatibility check
    public static bool IsBeta()
    {
        var v = BetaMainCompatibility.Version;
        Mkt_init.Logger.Info(v.ToString());
        return (v.Major, v.Minor, v.Patch).CompareTo((0, 111, 0)) >= 0;
    }
}