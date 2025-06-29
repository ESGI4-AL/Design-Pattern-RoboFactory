namespace RoboFactory.Commands.CommandTypes;

public class StocksCommand(CommandService service, Dictionary<RobotTemplate, int> request) : CommandBase(service, request)
{
    public override void Execute()
    {
        Service.Stocks();
    }
}