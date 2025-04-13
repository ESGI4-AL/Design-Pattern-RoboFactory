namespace RoboFactory;

public class RobotComponent(string name, string type, int quantity, bool requireSystem = false)
{
    public string Name { get; set; } = name;
    public int Quantity { get; set; } = quantity;
    public string Type { get; set; } = type;
    public bool RequireSystem { get; set; } = requireSystem;

    public override string ToString()
    {
        return $"{Quantity} - {Name}";
    }
}
