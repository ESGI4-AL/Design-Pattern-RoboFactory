namespace RoboFactory.Factories;

public static class FactorySelector
{
    public static IRobotFactory GetFactory(ECategory category)
    {
        return category switch
        {
            ECategory.Military => new MilitaryFactory(),
            ECategory.Domestic => new DomesticFactory(),
            ECategory.Industrial => new IndustrialFactory(),
            _ => throw new ArgumentException("No factory exists for this category")
        };
    }
}