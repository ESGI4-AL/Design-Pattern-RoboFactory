using RoboFactory.Utils;

namespace RoboFactory.Models;

public class Arms : InventoryItem
{
    public Arms(ECategory category) : base(
        RobotComponentResolver.ResolveCategoryType(category, EItemType.Arms), 
        category, 
        EItemType.Arms
    ) {}

    public override Arms PopItem()
    {
        if (HasNext()) return (Arms)base.PopItem();
        return this;
    }
}