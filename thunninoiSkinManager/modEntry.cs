using System.Reflection;
using Godot;
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using thunninoiSkinManager.thunninoiSkinManagerCode;

namespace thunninoiSkinManager;

[ModInitializer(nameof(Initialize))]
public partial class modEntry : Node
{
    public const string
        ModId = "thunninoi.SkinManager"; //At the moment, this is used only for the Logger and harmony names.

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);
    
    public static void PrintChildren(Node currentNode, int depth = 0)
    {
        if (currentNode == null) return;
        
        string prefix = depth == 0 ? "" : " " + new string('-', depth) + " ";
        string text = $"{prefix}{currentNode.Name}";
        Logger.Info(text);

        foreach (Node child in currentNode.GetChildren())
        {
            PrintChildren(child, depth + 1);
        }
    }
    
    public static string? ModDirectory = Path.GetDirectoryName(typeof(modEntry).Assembly.Location);

    public static void Initialize()
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        Harmony harmony = new(ModId);

        ScriptManagerBridge.LookupScriptsInAssembly(executingAssembly);
        harmony.PatchAll(executingAssembly);
    }
}