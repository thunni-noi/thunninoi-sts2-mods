using System.Reflection;
using BaseLib.Config;
using ChenIronclad.ChenIroncladCode;
using Godot;
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace ChenIronclad;

[ModInitializer(nameof(Initialize))]
public partial class ChenIroncladInit : Node
{
    public const string ModId = "ChenIronclad"; //Used for resource filepath

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);
        ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());
        ModConfigRegistry.Register(ModId, new ChenConfig());
        harmony.PatchAll();
    }
}