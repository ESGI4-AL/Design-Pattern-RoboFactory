namespace RoboFactory.Commands.CommandTypes;

public class InstructionsCommand : CommandBase
{
    public InstructionsCommand(InventoryCommandService service, Dictionary<(ECategory, EItemType), int> request) : base(service, request) { }
    
    public override void Execute()
    {
        Service.Instructions(Request);
    }
}