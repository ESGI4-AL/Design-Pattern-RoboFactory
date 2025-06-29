namespace RoboFactory.Utils;

public static class RobotComponentResolver
{
    public static string ResolveCategoryType(ECategory category, EItemType type)
    {
        return (category, type) switch
        {
            (ECategory.Military, EItemType.Core) => "Core_CM1",
            (ECategory.Domestic, EItemType.Core) => "Core_CD1",
            (ECategory.Industrial, EItemType.Core) => "Core_CI1",

            (ECategory.Military, EItemType.Generator) => "Generator_GM1",
            (ECategory.Domestic, EItemType.Generator) => "Generator_GD1",
            (ECategory.Industrial, EItemType.Generator) => "Generator_GI1",

            (ECategory.Military, EItemType.Arms) => "Arms_AM1",
            (ECategory.Domestic, EItemType.Arms) => "Arms_AD1",
            (ECategory.Industrial, EItemType.Arms) => "Arms_AI1",

            (ECategory.Military, EItemType.Legs) => "Legs_LM1",
            (ECategory.Domestic, EItemType.Legs) => "Legs_LD1",
            (ECategory.Industrial, EItemType.Legs) => "Legs_LI1",

            (ECategory.Generalist, EItemType.System) => "System_SB1",
            (ECategory.Military, EItemType.System) => "System_SM1",
            (ECategory.Domestic, EItemType.System) => "System_SD1",
            (ECategory.Industrial, EItemType.System) => "System_SI1",
            
            _ => throw new ArgumentException($"Unsupported combination: {category}, {type}")
        };
    }
    
    public static ECategory ResolveName(string itemName)
    {
        return itemName switch
        {
            "Core_CM1" => ECategory.Military,
            "Core_CD1" => ECategory.Domestic,
            "Core_CI1" => ECategory.Industrial,

            "Generator_GM1" => ECategory.Military,
            "Generator_GD1" => ECategory.Domestic,
            "Generator_GI1" => ECategory.Industrial,

            "Arms_AM1" => ECategory.Military,
            "Arms_AD1" => ECategory.Domestic,
            "Arms_AI1" => ECategory.Industrial,

            "Legs_LM1" => ECategory.Military,
            "Legs_LD1" => ECategory.Domestic,
            "Legs_LI1" => ECategory.Industrial,

            "System_SB1" => ECategory.Generalist,
            "System_SM1" => ECategory.Military,
            "System_SD1" => ECategory.Domestic,
            "System_SI1" => ECategory.Industrial,

            _ => throw new ArgumentException($"`{itemName}` is not a recognized component")
        };
    }
}