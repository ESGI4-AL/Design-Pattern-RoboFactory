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

    public bool HasNext()
    {
        return Next != null;
    }

    public void AddItem(InventoryItem item)
    {
        if (Next == null) Next = item;
        else Next.AddItem(item);
    }
    
    public virtual InventoryItem PopItem()
    {
        if (! HasNext())
            return this;

        var last = Next.PopItem();
        
        if (! Next.HasNext())
            Next = null;

        return last;
    }

    public int CountItem()
    {
        if (HasNext()) return 1 + Next.CountItem();
        return 1;
    }
}