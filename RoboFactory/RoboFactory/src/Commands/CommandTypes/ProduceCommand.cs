namespace RoboFactory.Commands.CommandTypes;

public class ProduceCommand : CommandBase
{
    public ProduceCommand(InventoryCommandService service, Dictionary<(ECategory, EItemType), int> request) : base(service, request){ }
    
    public override void Execute()
    {
        Service.Produce(Request);
    }
}