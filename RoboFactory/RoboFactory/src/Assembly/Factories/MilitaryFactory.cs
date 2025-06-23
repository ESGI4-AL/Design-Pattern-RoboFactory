using RoboFactory.Utils;

namespace RoboFactory.Factories;

public class MilitaryFactory : IRobotFactory
{
    public InventoryItem CreateRobot()
    {
        return new InventoryItem(RobotResolver.ResolveCategoryType(ECategory.Military), ECategory.Military, EItemType.Robot);
    }
}