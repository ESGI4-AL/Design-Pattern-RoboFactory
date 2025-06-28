using RoboFactory.Utils;

namespace RoboFactory.Models;

public class Robot : InventoryItem
{
    public Core core;
    public Generator generator;
    public Arms arms;
    public Legs legs;

    public Robot(ECategory category)
        : base(RobotResolver.ResolveCategoryType(category), category, EItemType.Robot) { }
}