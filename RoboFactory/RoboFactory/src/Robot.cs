namespace RoboFactory;

public class Robot(
    string name,
    int quantity,
    string coreName,
    string generatorName,
    string armsName,
    string legsName,
    string systemName)
{
    public string Name { get; set; } = name;
    public int Quantity { get; set; } = quantity;

    public string CoreName { get; set; } = coreName;
    public string GeneratorName { get; set; } = generatorName;
    public string ArmsName { get; set; } = armsName;
    public string LegsName { get; set; } = legsName;
    public string SystemName { get; set; } = systemName;

    public Dictionary<string, int> GetRequiredPieces()
    {
        return new Dictionary<string, int>
        {
            { CoreName, 1 * Quantity },
            { GeneratorName, 1 * Quantity },
            { ArmsName, 1 * Quantity },
            { LegsName, 1 * Quantity },
            { SystemName, 1 * Quantity }
        };
    }
    
    public override string ToString()
    {
        return $"{Quantity} - {Name}";
    }
}
