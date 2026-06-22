using System.Reflection;
using BaseLib.Config;
using CEdefect.CEdefectCode;
using Godot;
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace CEdefect;

[ModInitializer(nameof(Initialize))]
public partial class CE_Init : Node
{
    public const string ModId = "thunninoi.CivilightEternaDefect"; //Used for resource filepath

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);
    
    public static string? modDirectory = Path.GetDirectoryName(typeof(CE_Init).Assembly.Location);

    public static void Initialize()
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        Harmony harmony = new(ModId);
        ScriptManagerBridge.LookupScriptsInAssembly(executingAssembly);
        ModConfigRegistry.Register(ModId, new CE_Config());
        harmony.PatchAll(executingAssembly);
        //Logger.Info("Mod loaded from " + modDirectory);
    }
}

