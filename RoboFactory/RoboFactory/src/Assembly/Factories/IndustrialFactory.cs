using RoboFactory.Utils;

namespace RoboFactory.Factories;

public class IndustrialFactory : IRobotFactory
{
    public InventoryItem CreateRobot()
    {
        return new InventoryItem(RobotResolver.ResolveCategoryType(ECategory.Industrial), ECategory.Industrial, EItemType.Robot);
    }
}