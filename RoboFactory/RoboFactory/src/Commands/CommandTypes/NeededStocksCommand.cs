namespace RoboFactory.Commands.CommandTypes;

public class NeededStocksCommand : CommandBase
{
    public NeededStocksCommand(InventoryCommandService service, Dictionary<(ECategory, EItemType), int> request) : base(service, request) { }
    
    public override void Execute()
    {
        Service.NeededStocks(Request);
    }
}