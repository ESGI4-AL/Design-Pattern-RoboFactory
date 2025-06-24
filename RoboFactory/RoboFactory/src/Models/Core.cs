using RoboFactory.Utils;

namespace RoboFactory.Models;

public class Core : InventoryItem
{
    public Core(ECategory category) : base(
            RobotComponentResolver.ResolveCategoryType(category, EItemType.Core), 
            category, 
            EItemType.Core
        ) {}

    public override Core PopItem()
    {
        if (HasNext()) return (Core)base.PopItem();
        return this;
    }
}