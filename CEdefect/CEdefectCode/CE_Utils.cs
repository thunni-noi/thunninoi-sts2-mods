using Godot;
using MegaCrit.Sts2.Core.Models;

namespace CEdefect.CEdefectCode;

public class CE_Utils
{
    public static void Logger(string msg)
    {
        if (!CE_Config.DebugMode) return;
        CE_Init.Logger.Info(msg);
    }
    
    public static bool IsDefect(CharacterModel instance)
    {
        ModelId modelId = ((AbstractModel)instance).Id;
        String modelEntry = modelId.Entry;
        Logger($"IsDefect: {modelEntry}");
        if (string.IsNullOrWhiteSpace(modelEntry)) return false;
        if (!string.Equals(modelEntry, "defect", StringComparison.OrdinalIgnoreCase)) return false;
        return true;
    }

    public static bool ResourceAvailable(string resourcePath)
    {
        if (!ResourceLoader.Exists(resourcePath))
        {
            Logger($" Cannot find path : {resourcePath}");
            return false;
        }
        return true;
    }
}