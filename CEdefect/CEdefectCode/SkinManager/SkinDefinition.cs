namespace CEdefect.CEdefectCode.SkinManager;

public class SkinDefinition
{
    // internal stuff
    public string SkinId  { get; init; }
    public string SkinName { get; init; }
    
    // character visuals
    public string? CombatScene { get; init; }
    public string? MerchantScene { get; init; }
    public string? RestScene { get; init; }
    
    // character select
    public string? CharacterSelectBg { get; init; }
    public string? CharacterSelectPortrait { get; init; }
    
    public string? CharacterIcon { get; init; }
    public string? CharacterIconOutline { get; init; }
    public string? CharacterIconScene { get; init; }
    public string? CharacterMapIcon { get; init; }
    
    public string? ArmPointing { get; init; }
    public string? ArmRock { get; init; }
    public string? ArmPaper { get; init; }
    public string? ArmScissors { get; init; }
}