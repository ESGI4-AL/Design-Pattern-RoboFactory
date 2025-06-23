using System.Text;
using RoboFactory.Factories;

namespace RoboFactory;

public class FactoryInventory
{
    private readonly List<InventoryItem> _items;

    public FactoryInventory()
    {
        _items = new List<InventoryItem>();
        InitializeInventory();
    }
    
    /**
     * Ajout des pièces de robots et des robots entiers à l'inventaire
     */
    private void InitializeInventory()
    {
        AddItem(FactorySelector.GetFactory(ECategory.Military).CreateRobot());
        AddItem(FactorySelector.GetFactory(ECategory.Military).CreateRobot());
        AddItem(FactorySelector.GetFactory(ECategory.Domestic).CreateRobot());
        AddItem(FactorySelector.GetFactory(ECategory.Domestic).CreateRobot());
        AddItem(FactorySelector.GetFactory(ECategory.Industrial).CreateRobot());
        AddItem(new InventoryItem("Core_CM1", ECategory.Military, EItemType.Core));
        AddItem(new InventoryItem("Core_CM1", ECategory.Military, EItemType.Core));
    }
    
    /**
     * Ajoute un article à l'inventaire.
     */
    public void AddItem(InventoryItem item)
    {
        foreach (var inventoryItem in _items)
        {
            if ((inventoryItem.Category, inventoryItem.ItemType) == (item.Category, item.ItemType))
            {
                inventoryItem.AddItem(item);
                return;
            }
        }
        _items.Add(item);
    }

    private InventoryItem? GetItem(ECategory category, EItemType itemType)
    {
        foreach (var item in _items)
        {
            if (item.Category == category && item.ItemType == itemType)
            {
                return item;
            }
        }
        return null;
    }
    
    /**
     * Affiche les pièces et robots en stock.
     */
    public string DisplayStock()
    {
        var result = new StringBuilder();
        
        // Affiche les robots en stock
        foreach (var inventoryItem in _items)
        {
            result.AppendLine($"{inventoryItem.CountItem()} {inventoryItem.Name}");
        }
        
        return result.ToString();
    }
    
    /**
     * Verifie si l'inventaire a suffisamment de stock pour assembler les robots demandés.
     */
    public bool HasSufficientStock(Dictionary<(ECategory, EItemType), int> requiredParts)
    {
        foreach (var part in requiredParts)
        {
            var (category, itemType) = part.Key;
            var quantity = part.Value;
            
            InventoryItem? inventoryItem = GetItem(category, itemType);
            if (inventoryItem == null || inventoryItem.CountItem() < quantity)
                return false;
        }
        return true;
    }
}
