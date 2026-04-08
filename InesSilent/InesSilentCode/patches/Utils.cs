using Godot;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;

namespace InesSilent.InesSilentCode;

public class Utils
{
    public static bool IsSilent(CharacterModel instance)
    {
        ModelId modelId = ((AbstractModel)instance).Id;
        String modelEntry = modelId.Entry;
        if (string.IsNullOrWhiteSpace(modelEntry)) return false;
        if (!string.Equals(modelEntry, "silent", StringComparison.OrdinalIgnoreCase)) return false;
        return true;
    }

    public static bool ResourceAvailable(string resourcePath)
    {
        if (!ResourceLoader.Exists(resourcePath))
        {
            Log.Error($"[CHEN] Cannot find path : {resourcePath}");
            return false;
        }
        return true;
    }
}