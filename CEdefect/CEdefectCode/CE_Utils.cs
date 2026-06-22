using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using thunninoiSkinManager.thunninoiSkinManagerCode;

namespace CEdefect.CEdefectCode;

public class CE_Utils
{
    public static void Logger(string msg)
    {
        if (!CE_Config.DebugMode) return;
        CE_Init.Logger.Info(msg);
    }

    public static bool IsUsingSkin() => (SkinRegistry.IsUsingSkin(ModelDb.Character<Defect>().Id, "ceterna") ||
                                         SkinRegistry.IsUsingSkin(ModelDb.Character<Defect>().Id, "ceterna2"));
    

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