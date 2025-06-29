namespace RoboFactory.Commands.CommandTypes;

public class NeededStocksCommand(CommandService service, Dictionary<RobotTemplate, int> request)
    : CommandBase(service, request)
{
    public override void Execute()
    {
        Service.NeededStocks(Request);
    }
}