using RoboFactory.Utils;

namespace RoboFactory.Factories;

public class DomesticFactory: IRobotFactory
{
    public InventoryItem CreateRobot()
    {
        return new InventoryItem(RobotResolver.ResolveCategoryType(ECategory.Domestic), ECategory.Domestic, EItemType.Robot);
    }
}