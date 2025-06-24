using RoboFactory.Utils;

namespace RoboFactory.Models;

public class Robot(
    ECategory category,
    Core core,
    Generator generator,
    Arms arms,
    Legs legs
) : InventoryItem(RobotResolver.ResolveCategoryType(category), category, EItemType.Robot);