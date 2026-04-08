using MegaCrit.Sts2.Core.Models;

namespace ChenIronclad.ChenIroncladCode;

public class Utils
{
    // Check if we need to replace the sprite or not
    public static bool IsIronclad(CharacterModel instance)
    {
        ModelId modelId = ((AbstractModel)instance).Id;
        String modelEntry = modelId.Entry;
        if (string.IsNullOrWhiteSpace(modelEntry)) return false;
        if (!string.Equals(modelEntry, "ironclad", StringComparison.OrdinalIgnoreCase)) return false;
        return true;
    }
}