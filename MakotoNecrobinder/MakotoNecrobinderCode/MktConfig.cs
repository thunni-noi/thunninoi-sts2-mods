using BaseLib.Config;

namespace MakotoNecrobinder.MakotoNecrobinderCode;

[ConfigHoverTipsByDefault]
public class MktConfig : SimpleModConfig
{
    [ConfigSlider(0, 200, 1, Format = "{0}%")]
    public static int EvokerSfxVolume { get; set; } = 100;

    public static bool UseMakotoCardFrame { get; set; } = true;
    public static bool UseMakotoEnergy { get; set; } = true;
    public static bool UseMakotoArms { get; set; } = true;
    public static bool UseMakotoTransition { get; set; } = true;
}