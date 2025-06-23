namespace RoboFactory;

public class InventoryItem
{
    public string Name;
    public ECategory Category;
    public EItemType ItemType;
    private InventoryItem? Next = null;

    public InventoryItem(string name, ECategory category, EItemType itemType)
    {
        Name = name;
        Category = category;
        ItemType = itemType;
    }

    public void AddItem(InventoryItem item)
    {
        if (Next == null) Next = item;
        else Next.AddItem(item);
    }
    
    public InventoryItem PopItem()
    {
        if (Next != null) return Next.PopItem();
        return this;
    }

    public int CountItem()
    {
        if (Next != null) return 1 + Next.CountItem();
        return 1;
    }
}