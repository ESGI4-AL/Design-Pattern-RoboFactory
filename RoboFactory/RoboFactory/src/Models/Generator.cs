using RoboFactory.Utils;

namespace RoboFactory.Models;

public class Generator : InventoryItem
{
    public Generator(ECategory category) : base(
        RobotComponentResolver.ResolveCategoryType(category, EItemType.Generator), 
        category, 
        EItemType.Generator
    ) {}

    public override Generator PopItem()
    {
        if (HasNext()) return (Generator)base.PopItem();
        return this;
    }
}