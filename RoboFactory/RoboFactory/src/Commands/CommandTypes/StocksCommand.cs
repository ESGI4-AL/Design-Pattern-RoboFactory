namespace RoboFactory.Commands.CommandTypes;

public class StocksCommand : CommandBase
{
    public StocksCommand(InventoryCommandService service, Dictionary<(ECategory, EItemType), int> request) : base(service, request) { }
    
    public override void Execute()
    {
        Service.Stocks();
    }
}