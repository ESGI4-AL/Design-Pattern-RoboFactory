using System.Text;
using RoboFactory.Models;

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
        var categories = new[] { ECategory.Military, ECategory.Domestic, ECategory.Industrial };

        foreach (var category in categories)
        {
            for (int i = 0; i < 5; i++)
            {
                var robotBuilder = new RobotBuilder(category);
                robotBuilder.SetCore(new Core(category));
                robotBuilder.SetGenerator(new Generator(category));
                robotBuilder.SetArms(new Arms(category));
                robotBuilder.SetLegs(new Legs(category));
                
                AddItem(robotBuilder.Build());
            }

            // Ajouter aussi des composants seuls
            AddItem(new CoreSystem(category));
            AddItem(new Core(category));
            AddItem(new Generator(category));
            AddItem(new Arms(category));
            AddItem(new Legs(category));

            for (int i = 0; i < 3; i++)
            {
                AddItem(new Core(category));
                AddItem(new Generator(category));
            }
        }
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
    
    private InventoryItem TakeItem(ECategory category, EItemType itemType)
    {
        var item = FindItem(category, itemType);
        if (item == null)
            throw new InvalidOperationException("Item not found");
        
        var popped = item.PopItem();
        
        if (item == popped)
            _items.Remove(item);

        return popped;
    }

    private InventoryItem? FindItem(ECategory category, EItemType itemType)
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
    
    public T TakeRobotComponent<T>(ECategory category) where T : InventoryItem
    {
        EItemType itemType = typeof(T) switch
        {
            Type t when t == typeof(CoreSystem) => EItemType.System,
            Type t when t == typeof(Core) => EItemType.Core,
            Type t when t == typeof(Arms) => EItemType.Arms,
            Type t when t == typeof(Legs) => EItemType.Legs,
            Type t when t == typeof(Generator) => EItemType.Generator,
            _ => throw new InvalidOperationException("Unknown component type")
        };
        
        InventoryItem item = TakeItem(category, itemType);
        
        return item as T ?? throw new InvalidOperationException("Unknown component type");
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
            
            InventoryItem? inventoryItem = FindItem(category, itemType);
            if (inventoryItem == null || inventoryItem.CountItem() < quantity)
                return false;
        }
        return true;
    }
}
