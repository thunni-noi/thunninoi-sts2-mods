using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;

namespace ChenIronclad.ChenIroncladCode;

public class ChenUtils
{
    // Check if we need to replace the sprite or not
    public static bool IsIronclad(CharacterModel instance)
    {
        ModelId modelId = ((AbstractModel)instance).Id;
        String modelEntry = modelId.Entry;
        if (ChenConfig.DebugMode) Log.Info($"[CHEN] Utils has been called to check {modelId.Entry} - {modelEntry}");
        if (string.IsNullOrWhiteSpace(modelEntry)) return false;
        if (!string.Equals(modelEntry, "ironclad", StringComparison.OrdinalIgnoreCase)) return false;
        return true;
    }
}