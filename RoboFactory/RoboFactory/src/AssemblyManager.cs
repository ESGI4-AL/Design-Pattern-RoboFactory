using System.Text;
using RoboFactory.Utils;

namespace RoboFactory;

public class AssemblyManager(FactoryInventory inventory)
{
    /**
     * Calcule les pièces nécessaires pour assembler les robots demandés.
     * Retourne un dictionnaire avec toutes les pièces nécessaires et leur quantité.
     */
    public Dictionary<string, int> CalculateRequiredPieces(Dictionary<string, int> requestedRobots)
    {
        Dictionary<string, int> requiredPieces = new Dictionary<string, int>();
        
        foreach (var robotRequest in requestedRobots)
        {
            Robot? robot = inventory.GetRobot(robotRequest.Key);
            if (robot == null) continue;
            
            Robot tempRobot = new Robot(robot.Name, robotRequest.Value, 
                robot.CoreName, robot.GeneratorName, 
                robot.ArmsName, robot.LegsName, robot.SystemName);
            
            var robotParts = tempRobot.GetRequiredPieces();
                
            // Ajouter au total
            foreach (var part in robotParts)
            {
                if (requiredPieces.ContainsKey(part.Key))
                    requiredPieces[part.Key] += part.Value;
                else
                    requiredPieces[part.Key] = part.Value;
            }
        }
            
        return requiredPieces;
    }

    /**
     * Liste les stocks nécessaires pour assembler les robots demandés.
     */
    public string GenerateNeededStocks(Dictionary<string, int> requestedRobots)
    {
        var output = new StringBuilder();
        
        foreach (var robotRequest in requestedRobots)
        {
            Robot? robot = inventory.GetRobot(robotRequest.Key);
            if (robot == null) continue;
                
            output.AppendLine($"{robotRequest.Value} {robotRequest.Key} :");
                
            output.AppendLine($"{robotRequest.Value} {robot.CoreName}");
            output.AppendLine($"{robotRequest.Value} {robot.GeneratorName}");
            output.AppendLine($"{robotRequest.Value} {robot.ArmsName}");
            output.AppendLine($"{robotRequest.Value} {robot.LegsName}");
            output.AppendLine($"{robotRequest.Value} {robot.SystemName}");
        }
            
        // Total des pièces nécessaires
        output.AppendLine("Total :");
        var totalParts = CalculateRequiredPieces(requestedRobots);
        foreach (var part in totalParts)
        {
            output.AppendLine($"{part.Value} {part.Key}");
        }
            
        return output.ToString();
    }
    
    /**
     * Génère les instructions d'assemblage pour les robots demandés.
     */
    public string GenerateAssemblyInstructions(Dictionary<string, int> robotsRequested)
    {
        var output = new StringBuilder();
            
        foreach (var robotRequest in robotsRequested)
        {
            Robot? robot = inventory.GetRobot(robotRequest.Key);
            if (robot == null) continue;
                
            for (int i = 0; i < robotRequest.Value; i++)
            {
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Producing)} {robot.Name}");
                    
                // Sort les pièces du stock
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.GetOutStock)} 1 {robot.CoreName}");
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.GetOutStock)} 1 {robot.GeneratorName}");
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.GetOutStock)} 1 {robot.ArmsName}");
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.GetOutStock)} 1 {robot.LegsName}");
                    
                // Installation du système sur le module principal
                RobotComponent? coreRobotComponent = inventory.GetPiece(robot.CoreName);
                if (coreRobotComponent != null && coreRobotComponent.RequireSystem)
                {
                    output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Install)} {robot.SystemName} {robot.CoreName}");
                }
                    
                // Assemblage des pièces
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Assemble)} TMP1 {robot.CoreName} {robot.GeneratorName}");
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Assemble)} TMP1 {robot.ArmsName}");
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Assemble)} TMP3 [TMP1,{robot.ArmsName}] {robot.LegsName}");
                    
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Finished)} {robot.Name}");
            }
        }
            
        return output.ToString();
    }
}
