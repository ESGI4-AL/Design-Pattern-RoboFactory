namespace RoboFactory.Commands.CommandTypes;

public class ProduceCommand(CommandService service, Dictionary<RobotTemplate, int> request) : CommandBase(service, request)
{
    public override void Execute()
    {
        Service.Produce(Request);
    }
}