using RoboFactory.Utils;

namespace RoboFactory.Models;

public class CoreSystem : InventoryItem
{
    public CoreSystem(ECategory category) : base(
        RobotComponentResolver.ResolveCategoryType(category, EItemType.System), 
        category, 
        EItemType.System
    ) {}

    public override CoreSystem PopItem()
    {
        if (HasNext()) return (CoreSystem)base.PopItem();
        return this;
    }
}