using System.Reflection;
using BaseLib.Config;
using CEdefect.CEdefectCode;
using CEdefect.CEdefectCode.SkinManager;
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

    public static void Initialize()
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        Harmony harmony = new(ModId);
        ScriptManagerBridge.LookupScriptsInAssembly(executingAssembly);
        ModConfigRegistry.Register(ModId, new CE_Config());
        SkinRegistry.RegisterAll();
        SkinRegistry.Load();
        harmony.PatchAll(executingAssembly);
    }
}