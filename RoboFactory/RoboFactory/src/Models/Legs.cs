using RoboFactory.Utils;

namespace RoboFactory.Models;

public class Legs : InventoryItem
{
    public Legs(ECategory category) : base(
        RobotComponentResolver.ResolveCategoryType(category, EItemType.Legs), 
        category, 
        EItemType.Legs
    ) {}

    public override Legs PopItem()
    {
        if (HasNext()) return (Legs)base.PopItem();
        return this;
    }
}