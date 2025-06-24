using System.Text;
using RoboFactory.Models;
using RoboFactory.Utils;

namespace RoboFactory.Commands;

public class InventoryCommandService
{
    private readonly FactoryInventory _inventory;
    private readonly AssemblyManager _assemblyManager;
    public string Output = "";

    public InventoryCommandService(FactoryInventory inventory,  AssemblyManager assemblyManager)
    {
        _inventory = inventory;
        _assemblyManager = assemblyManager;
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
        var totalParts = _assemblyManager.CalculateRequiredPieces(request);
        foreach (var part in totalParts)
        {
            var (category, type) = part.Key;
            outputBuilder.AppendLine($"{part.Value} {RobotComponentResolver.ResolveCategoryType(category, type)}");
        }
        
        Output = outputBuilder.ToString();
    }

    public void Instructions(Dictionary<(ECategory, EItemType), int> request)
    {
        Output = _assemblyManager.GenerateAssemblyInstructions(request);
    }

    public void Verify(Dictionary<(ECategory, EItemType), int> request)
    {
        Dictionary<(ECategory, EItemType), int> requiredPieces = _assemblyManager.CalculateRequiredPieces(request);
            
        // Vérifie si les stocks sont suffisants
        if (_inventory.HasSufficientStock(requiredPieces))
            Output = SystemResponseTypeUtils.ToString(SystemResponseType.Available);
        else
            Output = SystemResponseTypeUtils.ToString(SystemResponseType.Unavailable);
    }

    public void Produce(Dictionary<(ECategory, EItemType), int> request)
    {
        // Calcule les pièces nécessaires
        Dictionary<(ECategory, EItemType), int> requiredPieces = _assemblyManager.CalculateRequiredPieces(request);
        
        // Vérifie si les stocks sont suffisants
        if (! _inventory.HasSufficientStock(requiredPieces))
        {
            throw new ArgumentException("Insufficient stock for production");
        }

        foreach (var robotRequest in request)
        {
            ECategory category = robotRequest.Key.Item1;
            int quantity = robotRequest.Value;

            for (int i = 0; i < quantity; i++)
            {
                var system = _inventory.TakeRobotComponent<CoreSystem>(category);
                var core = _inventory.TakeRobotComponent<Core>(category);
                var generator = _inventory.TakeRobotComponent<Generator>(category);
                var arms = _inventory.TakeRobotComponent<Arms>(category);
                var legs = _inventory.TakeRobotComponent<Legs>(category);
                
                Robot robot = _assemblyManager.AssembleRobot(category, system, core, generator, arms, legs);
                
                _inventory.AddItem(robot);
            }
        }
        
        Output = SystemResponseTypeUtils.ToString(SystemResponseType.StockUpdated);
    }
}