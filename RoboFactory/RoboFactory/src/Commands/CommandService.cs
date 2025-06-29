using System.Text;
using RoboFactory.Models;
using RoboFactory.Utils;

namespace RoboFactory.Commands;

public class CommandService(FactoryInventory inventory, AssemblyManual assemblyManual)
{
    public string Output = "";

    public void Stocks()
    {
        Output = inventory.DisplayStock();
    }

    public void NeededStocks(Dictionary<RobotTemplate, int> request)
    {
        var outputBuilder = new StringBuilder();
        
        foreach (var robotRequest in request)
        {
            var template = robotRequest.Key;
            
            outputBuilder.AppendLine($"{robotRequest.Value} {template.name} :");
                
            outputBuilder.AppendLine($"{robotRequest.Value} {RobotComponentResolver.ResolveCategoryType(template.systemCategory, EItemType.System)}");
            outputBuilder.AppendLine($"{robotRequest.Value} {RobotComponentResolver.ResolveCategoryType(template.coreCategory, EItemType.Core)}");
            outputBuilder.AppendLine($"{robotRequest.Value} {RobotComponentResolver.ResolveCategoryType(template.generatorCategory, EItemType.Generator)}");
            outputBuilder.AppendLine($"{robotRequest.Value} {RobotComponentResolver.ResolveCategoryType(template.armsCategory, EItemType.Arms)}");
            outputBuilder.AppendLine($"{robotRequest.Value} {RobotComponentResolver.ResolveCategoryType(template.legsCategory, EItemType.Legs)}");
        }
            
        // Total des pièces nécessaires
        outputBuilder.AppendLine("Total :");
        var totalParts = assemblyManual.CalculateRequiredPieces(request);
        foreach (var part in totalParts)
        {
            var (category, type) = part.Key;
            outputBuilder.AppendLine($"{part.Value} {RobotComponentResolver.ResolveCategoryType(category, type)}");
        }
        
        Output = outputBuilder.ToString();
    }

    public void Instructions(Dictionary<RobotTemplate, int> request)
    {
        Output = assemblyManual.GenerateAssemblyInstructions(request);
    }

    public void Verify(Dictionary<RobotTemplate, int> request)
    {
        Dictionary<(ECategory, EItemType), int> requiredPieces = assemblyManual.CalculateRequiredPieces(request);
            
        // Vérifie si les stocks sont suffisants
        if (inventory.HasSufficientStock(requiredPieces))
            Output = SystemResponseTypeUtils.ToString(SystemResponseType.Available);
        else
            Output = SystemResponseTypeUtils.ToString(SystemResponseType.Unavailable);
    }

    public void Produce(Dictionary<RobotTemplate, int> request)
    {
        // Calcule les pièces nécessaires
        Dictionary<(ECategory, EItemType), int> requiredPieces = assemblyManual.CalculateRequiredPieces(request);
        
        // Vérifie si les stocks sont suffisants
        if (! inventory.HasSufficientStock(requiredPieces))
        {
            throw new ArgumentException("Insufficient stock for production");
        }

        foreach (var robotRequest in request)
        {
            RobotTemplate template = robotRequest.Key;
            RobotBuilder robotBuilder = new RobotBuilder(template.robotCategory);
            int quantity = robotRequest.Value;
            
            for (int i = 0; i < quantity; i++)
            {
                var system = inventory.TakeRobotComponent<CoreSystem>(template.systemCategory);
                robotBuilder.SetCore(inventory.TakeRobotComponent<Core>(template.coreCategory));
                robotBuilder.SetGenerator(inventory.TakeRobotComponent<Generator>(template.generatorCategory));
                robotBuilder.SetArms(inventory.TakeRobotComponent<Arms>(template.armsCategory));
                robotBuilder.SetLegs(inventory.TakeRobotComponent<Legs>(template.legsCategory));
                var robot = robotBuilder.Build();
                
                inventory.AddItem(robot);
            }
        }
        
        Output = SystemResponseTypeUtils.ToString(SystemResponseType.StockUpdated);
    }

    public void AddTemplate(Dictionary<RobotTemplate, int> request)
    {
        foreach (var template in request.Keys)
        {
            assemblyManual.AddTemplate(template);
        }
        Output = SystemResponseTypeUtils.ToString(SystemResponseType.TemplatesUpdated);
    }

    public RobotTemplate GetTemplate(string name)
    {
        return assemblyManual.GetTemplate(name);
    }
}