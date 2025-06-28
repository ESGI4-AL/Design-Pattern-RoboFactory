using System.Text;
using RoboFactory.Utils;

namespace RoboFactory;

public class AssemblyManual()
{
    /**
     * Calcule les pièces nécessaires pour assembler les robots demandés.
     * Retourne un dictionnaire avec toutes les pièces nécessaires et leur quantité.
     */
    public Dictionary<(ECategory, EItemType), int> CalculateRequiredPieces(Dictionary<(ECategory, EItemType), int> requestedRobots)
    {
        Dictionary<(ECategory, EItemType), int> requiredPieces = new Dictionary<(ECategory, EItemType), int>();
        
        foreach (var robotRequest in requestedRobots)
        {
            ECategory category = robotRequest.Key.Item1;
            requiredPieces[(category, EItemType.System)] = robotRequest.Value;
            requiredPieces[(category, EItemType.Core)] = robotRequest.Value;
            requiredPieces[(category, EItemType.Generator)] = robotRequest.Value;
            requiredPieces[(category, EItemType.Arms)] = robotRequest.Value;
            requiredPieces[(category, EItemType.Legs)] = robotRequest.Value;
        }
            
        return requiredPieces;
    }
    
    /**
     * Génère les instructions d'assemblage pour les robots demandés.
     */
    public string GenerateAssemblyInstructions(Dictionary<(ECategory, EItemType), int> robotsRequested)
    {
        var output = new StringBuilder();
            
        foreach (var robotRequest in robotsRequested)
        {
            var (category, type) = robotRequest.Key;
            for (int i = 0; i < robotRequest.Value; i++)
            {
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Producing)} {RobotResolver.ResolveCategoryType(category)}");
                    
                // Sort les pièces du stock
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.GetOutStock)} 1 {RobotComponentResolver.ResolveCategoryType(category, EItemType.Core)}");
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.GetOutStock)} 1 {RobotComponentResolver.ResolveCategoryType(category, EItemType.Generator)}");
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.GetOutStock)} 1 {RobotComponentResolver.ResolveCategoryType(category, EItemType.Arms)}");
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.GetOutStock)} 1 {RobotComponentResolver.ResolveCategoryType(category, EItemType.Legs)}");
                
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Install)} {RobotComponentResolver.ResolveCategoryType(category, EItemType.System)} {RobotComponentResolver.ResolveCategoryType(category, EItemType.Core)}");
                
                // Assemblage des pièces
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Assemble)} TMP1 {RobotComponentResolver.ResolveCategoryType(category, EItemType.Core)} {RobotComponentResolver.ResolveCategoryType(category, EItemType.Generator)} ");
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Assemble)} TMP1 {RobotComponentResolver.ResolveCategoryType(category, EItemType.Arms)}");
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Assemble)} TMP3 [TMP1,{RobotComponentResolver.ResolveCategoryType(category, EItemType.Arms)}] {RobotComponentResolver.ResolveCategoryType(category, EItemType.Legs)}");
                    
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Finished)} {RobotResolver.ResolveCategoryType(category)}");
            }
        }
            
        return output.ToString();
    }
}
