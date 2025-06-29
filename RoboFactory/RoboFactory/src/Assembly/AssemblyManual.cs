using System.Text;
using RoboFactory.Utils;

namespace RoboFactory;

public class AssemblyManual
{
    private List<RobotTemplate> _templates;

    public AssemblyManual()
    {
        _templates = new List<RobotTemplate>();
        AddTemplate(new RobotTemplate("XM-1", ECategory.Military, ECategory.Generalist, ECategory.Military, ECategory.Military, ECategory.Military, ECategory.Military));
        AddTemplate(new RobotTemplate("RD-1", ECategory.Domestic, ECategory.Generalist, ECategory.Domestic, ECategory.Domestic, ECategory.Domestic, ECategory.Domestic));
        AddTemplate(new RobotTemplate("WI-1", ECategory.Industrial, ECategory.Generalist, ECategory.Industrial, ECategory.Industrial, ECategory.Industrial, ECategory.Industrial));
    }
    
    /**
     * Calcule les pièces nécessaires pour assembler les robots demandés.
     * Retourne un dictionnaire avec toutes les pièces nécessaires et leur quantité.
     */
    public Dictionary<(ECategory, EItemType), int> CalculateRequiredPieces(Dictionary<RobotTemplate, int> requestedRobots)
    {
        Dictionary<(ECategory, EItemType), int> requiredPieces = new Dictionary<(ECategory, EItemType), int>();
        
        foreach (var robotRequest in requestedRobots)
        {
            ECategory category = robotRequest.Key.robotCategory;
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
    public string GenerateAssemblyInstructions(Dictionary<RobotTemplate, int> robotsRequested)
    {
        var output = new StringBuilder();
            
        foreach (var robotRequest in robotsRequested)
        {
            var template = robotRequest.Key;
            for (int i = 0; i < robotRequest.Value; i++)
            {
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Producing)} {template.name}");
                    
                // Sort les pièces du stock
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.GetOutStock)} 1 {RobotComponentResolver.ResolveCategoryType(template.coreCategory, EItemType.Core)}");
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.GetOutStock)} 1 {RobotComponentResolver.ResolveCategoryType(template.generatorCategory, EItemType.Generator)}");
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.GetOutStock)} 1 {RobotComponentResolver.ResolveCategoryType(template.armsCategory, EItemType.Arms)}");
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.GetOutStock)} 1 {RobotComponentResolver.ResolveCategoryType(template.legsCategory, EItemType.Legs)}");
                
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Install)} {RobotComponentResolver.ResolveCategoryType(template.systemCategory, EItemType.System)} {RobotComponentResolver.ResolveCategoryType(template.coreCategory, EItemType.Core)}");
                
                // Assemblage des pièces
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Assemble)} TMP1 {RobotComponentResolver.ResolveCategoryType(template.coreCategory, EItemType.Core)} {RobotComponentResolver.ResolveCategoryType(template.generatorCategory, EItemType.Generator)} ");
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Assemble)} TMP1 {RobotComponentResolver.ResolveCategoryType(template.armsCategory, EItemType.Arms)}");
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Assemble)} TMP3 [TMP1,{RobotComponentResolver.ResolveCategoryType(template.armsCategory, EItemType.Arms)}] {RobotComponentResolver.ResolveCategoryType(template.legsCategory, EItemType.Legs)}");
                    
                output.AppendLine($"{AssemblyInstructionTypeUtils.ToString(AssemblyInstructionType.Finished)} {template.name}");
            }
        }
            
        return output.ToString();
    }
    
    public RobotTemplate GetTemplate(string templateName)
    {
        return _templates.FirstOrDefault(t => t.name == templateName)
               ?? throw new ArgumentException("Invalid robot template");
    }

    public void AddTemplate(RobotTemplate request)
    {
        if (request.robotCategory == ECategory.Generalist)
        {
            request = request with { robotCategory = DetermineRobotCategory(request) };
        }
        
        bool isValid = request.robotCategory switch
        {
            ECategory.Industrial => AllAre(request, [ECategory.Industrial, ECategory.Generalist]),
            ECategory.Domestic => AllAre(request, [ECategory.Domestic, ECategory.Industrial, ECategory.Generalist]),
            ECategory.Military => SystemIs(request, [ECategory.Military, ECategory.Generalist])
                                  && AllExceptSystemAre(request, [ECategory.Military, ECategory.Industrial]),
            
            _ => false
        };

        if (! isValid) throw new ArgumentException("Invalid robot template");
        _templates.Add(request);
    }

    private bool AllAre(RobotTemplate t, ECategory[] categories)
    {
        return new [] {
            t.systemCategory,
            t.coreCategory,
            t.generatorCategory,
            t.armsCategory,
            t.legsCategory
        }.All(categories.Contains);
    }

    private bool SystemIs(RobotTemplate t, ECategory[] categories)
    {
        return categories.Contains(t.systemCategory);
    }
    
    private bool AllExceptSystemAre(RobotTemplate t, ECategory[] categories)
    {
        return new [] {
            t.coreCategory,
            t.generatorCategory,
            t.armsCategory,
            t.legsCategory
        }.All(categories.Contains);
    }

    private ECategory DetermineRobotCategory(RobotTemplate t)
    {
        if (SystemIs(t, [ECategory.Military, ECategory.Generalist]) &&
            AllExceptSystemAre(t, [ECategory.Military, ECategory.Industrial]))
            return ECategory.Military;

        if (AllAre(t, [ECategory.Industrial, ECategory.Generalist]))
            return ECategory.Industrial;

        if (AllAre(t, [ECategory.Domestic, ECategory.Industrial, ECategory.Generalist]))
            return ECategory.Domestic;

        throw new ArgumentException("Couldn't determine robot category");
    }
}
