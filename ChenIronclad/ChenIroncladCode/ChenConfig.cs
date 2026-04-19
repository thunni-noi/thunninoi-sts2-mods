using BaseLib.Config;

namespace ChenIronclad.ChenIroncladCode;

[ConfigHoverTipsByDefault]
internal class ChenConfig : SimpleModConfig
{
    public static bool UseChenCardFrame { get; set; } = true;
    public static bool UseChenEnergy { get; set; } = true;
    public static bool UseChenMultArm { get; set; } = true;
    public static bool UseChixiaoHellraiser { get; set; } = true;
    public static bool DebugMode { get; set; } = false;
}