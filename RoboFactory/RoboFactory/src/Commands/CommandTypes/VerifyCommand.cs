namespace RoboFactory.Commands.CommandTypes;

public class VerifyCommand : CommandBase
{
    public VerifyCommand(InventoryCommandService service, Dictionary<(ECategory, EItemType), int> request) : base(service, request) { }
    
    public override void Execute()
    {
        Service.Verify(Request);
    }
}