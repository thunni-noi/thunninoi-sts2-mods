using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;

namespace InesSilent.InesSilentCode;

public class InesUtils
{
    public static bool IsSilent(CharacterModel instance)
    {
        ModelId modelId = ((AbstractModel)instance).Id;
        String modelEntry = modelId.Entry;
        Logger($"Checking for entry - {modelId} , {modelEntry}");
        if (string.IsNullOrWhiteSpace(modelEntry)) return false;
        if (!string.Equals(modelEntry, "silent", StringComparison.OrdinalIgnoreCase)) return false;
        return true;
    }

    public static bool ResourceAvailable(string resourcePath)
    {
        Logger($"Checking for resource - {resourcePath}");
        if (!ResourceLoader.Exists(resourcePath))
        {
            Log.Error($"[INES] Cannot find path : {resourcePath}");
            return false;
        }
        return true;
    }

    public static void Logger(string msg)
    {
        if (!InesConfig.DebugMode) return;
        InesInitialize.Logger.Info("[INES] " + msg);
    }
}