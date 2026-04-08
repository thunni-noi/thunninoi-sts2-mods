using BaseLib.Config;

namespace InesSilent.InesSilentCode;

[HoverTipsByDefault]
public class InesConfig : SimpleModConfig
{
    public static bool UseInesCardFrame { get; set; } = true;
    public static bool UseInesEnergy { get; set; } = true;
    public static bool UseInesMultArm { get; set; } = true;
    public static bool UseWisadelePotion { get; set; } = true;
    public static bool RedShivColor { get; set; } = true;
}