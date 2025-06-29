namespace RoboFactory.Commands.CommandTypes;

public class VerifyCommand(CommandService service, Dictionary<RobotTemplate, int> request) : CommandBase(service, request)
{
    public override void Execute()
    {
        Service.Verify(Request);
    }
}