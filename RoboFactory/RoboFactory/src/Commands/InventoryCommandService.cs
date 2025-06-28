using System.Text;
using RoboFactory.Models;
using RoboFactory.Utils;

namespace RoboFactory.Commands;

public class InventoryCommandService
{
    private readonly FactoryInventory _inventory;
    private readonly AssemblyManual _assemblyManual;
    public string Output = "";

    public InventoryCommandService(FactoryInventory inventory,  AssemblyManual assemblyManual)
    {
        _inventory = inventory;
        _assemblyManual = assemblyManual;
    }

    public void Stocks()
    {
        Output = _inventory.DisplayStock();
    }

    public void NeededStocks(Dictionary<(ECategory, EItemType), int> request)
    {
        var outputBuilder = new StringBuilder();
        
        foreach (var robotRequest in request)
        {
            var (category, type) = robotRequest.Key;
            
            outputBuilder.AppendLine($"{robotRequest.Value} {RobotResolver.ResolveCategoryType(category)} :");
                
            outputBuilder.AppendLine($"{robotRequest.Value} {RobotComponentResolver.ResolveCategoryType(category, EItemType.System)}");
            outputBuilder.AppendLine($"{robotRequest.Value} {RobotComponentResolver.ResolveCategoryType(category, EItemType.Core)}");
            outputBuilder.AppendLine($"{robotRequest.Value} {RobotComponentResolver.ResolveCategoryType(category, EItemType.Generator)}");
            outputBuilder.AppendLine($"{robotRequest.Value} {RobotComponentResolver.ResolveCategoryType(category, EItemType.Arms)}");
            outputBuilder.AppendLine($"{robotRequest.Value} {RobotComponentResolver.ResolveCategoryType(category, EItemType.Legs)}");
        }
            
        // Total des pièces nécessaires
        outputBuilder.AppendLine("Total :");
        var totalParts = _assemblyManual.CalculateRequiredPieces(request);
        foreach (var part in totalParts)
        {
            var (category, type) = part.Key;
            outputBuilder.AppendLine($"{part.Value} {RobotComponentResolver.ResolveCategoryType(category, type)}");
        }
        
        Output = outputBuilder.ToString();
    }

    public void Instructions(Dictionary<(ECategory, EItemType), int> request)
    {
        Output = _assemblyManual.GenerateAssemblyInstructions(request);
    }

    public void Verify(Dictionary<(ECategory, EItemType), int> request)
    {
        Dictionary<(ECategory, EItemType), int> requiredPieces = _assemblyManual.CalculateRequiredPieces(request);
            
        // Vérifie si les stocks sont suffisants
        if (_inventory.HasSufficientStock(requiredPieces))
            Output = SystemResponseTypeUtils.ToString(SystemResponseType.Available);
        else
            Output = SystemResponseTypeUtils.ToString(SystemResponseType.Unavailable);
    }

    public void Produce(Dictionary<(ECategory, EItemType), int> request)
    {
        // Calcule les pièces nécessaires
        Dictionary<(ECategory, EItemType), int> requiredPieces = _assemblyManual.CalculateRequiredPieces(request);
        
        // Vérifie si les stocks sont suffisants
        if (! _inventory.HasSufficientStock(requiredPieces))
        {
            throw new ArgumentException("Insufficient stock for production");
        }

        foreach (var robotRequest in request)
        {
            ECategory category = robotRequest.Key.Item1;
            RobotBuilder robotBuilder = new RobotBuilder(category);
            int quantity = robotRequest.Value;
            
            for (int i = 0; i < quantity; i++)
            {
                var system = _inventory.TakeRobotComponent<CoreSystem>(category);
                robotBuilder.SetCore(_inventory.TakeRobotComponent<Core>(category));
                robotBuilder.SetGenerator(_inventory.TakeRobotComponent<Generator>(category));
                robotBuilder.SetArms(_inventory.TakeRobotComponent<Arms>(category));
                robotBuilder.SetLegs(_inventory.TakeRobotComponent<Legs>(category));
                var robot = robotBuilder.Build();
                
                _inventory.AddItem(robot);
            }
        }
        
        Output = SystemResponseTypeUtils.ToString(SystemResponseType.StockUpdated);
    }
}