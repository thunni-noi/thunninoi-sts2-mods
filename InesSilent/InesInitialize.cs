using System.Reflection;
using BaseLib.Config;
using Godot;
using Godot.Bridge;
using HarmonyLib;
using InesSilent.InesSilentCode;
using MegaCrit.Sts2.Core.Modding;

namespace InesSilent;

[ModInitializer(nameof(Initialize))]
public partial class InesInitialize : Node
{
	public const string ModId = "InesSilent"; //At the moment, this is used only for the Logger and harmony names.

	public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

	public static void Initialize()
	{
		Harmony harmony = new(ModId);
		ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());
		ModConfigRegistry.Register(ModId, new InesConfig());
		harmony.PatchAll();
	}
}
