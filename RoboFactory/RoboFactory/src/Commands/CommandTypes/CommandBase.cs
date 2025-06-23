namespace RoboFactory.Commands.CommandTypes;

public abstract class CommandBase
{
    protected InventoryCommandService Service { get; set; }
    protected Dictionary<(ECategory, EItemType), int> Request { get; set; }

    protected CommandBase(InventoryCommandService service, Dictionary<(ECategory, EItemType), int> request)
    {
        Service = service;
        Request = request;
    }
    
    public abstract void Execute();
}