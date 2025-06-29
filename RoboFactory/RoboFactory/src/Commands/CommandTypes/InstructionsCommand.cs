namespace RoboFactory.Commands.CommandTypes;

public class InstructionsCommand(CommandService service, Dictionary<RobotTemplate, int> request)
    : CommandBase(service, request)
{
    public override void Execute()
    {
        Service.Instructions(Request);
    }
}