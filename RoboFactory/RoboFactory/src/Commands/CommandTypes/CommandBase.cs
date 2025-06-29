namespace RoboFactory.Commands.CommandTypes;

public abstract class CommandBase
{
    protected CommandService Service { get; set; }
    protected Dictionary<RobotTemplate, int> Request { get; set; }

    protected CommandBase(CommandService service, Dictionary<RobotTemplate, int> request)
    {
        Service = service;
        Request = request;
    }
    
    public abstract void Execute();
}