namespace RoboFactory.Utils;

public static class RobotResolver
{
    public static string ResolveCategoryType(ECategory category)
    {
        return category switch
        {
            ECategory.Military => "XM-1",
            ECategory.Domestic => "RD-1",
            ECategory.Industrial => "WI-1",
            
            _ => throw new ArgumentException($"Unsupported category: {category}")
        };
    }
    
    public static (ECategory category, EItemType type) ResolveName(string itemName)
    {
        return itemName switch
        {
            "XM-1" => (ECategory.Military, EItemType.Robot),
            "RD-1" => (ECategory.Domestic, EItemType.Robot),
            "WI-1" => (ECategory.Industrial, EItemType.Robot),

            _ => throw new ArgumentException($"`{itemName}` is not a recognized robot")
        };
    }
}