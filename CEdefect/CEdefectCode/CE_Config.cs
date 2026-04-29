using BaseLib.Config;

namespace CEdefect.CEdefectCode;

[ConfigHoverTipsByDefault]
public class CE_Config : SimpleModConfig
{
    public static bool UseCivilightCardFrame { get; set; } = true;
    public static bool UseCivilightEnergy { get; set; } = true;
    public static bool UseCivilightMultArm { get; set; } = true;
    public static bool UseCivilightOrbs { get; set; } = true;
    public static bool UseCeCompatMode { get; set; } = false;
    public static bool DebugMode { get; set; } = false;
}