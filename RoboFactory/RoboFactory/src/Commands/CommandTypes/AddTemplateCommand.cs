namespace RoboFactory.Commands.CommandTypes;

public class AddTemplateCommand(CommandService service, Dictionary<RobotTemplate, int> request) : CommandBase(service, request)
{
    public override void Execute()
    {
        Service.AddTemplate(Request);
    }
}