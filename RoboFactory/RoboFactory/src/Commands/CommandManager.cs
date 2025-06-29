using RoboFactory.Commands.CommandTypes;
using RoboFactory.Utils;

namespace RoboFactory.Commands;

public class CommandManager
{
    private readonly CommandService _commandService;
    
    public CommandManager(CommandService commandService)
    {
        _commandService = commandService;
    }
    
    /**
     * Traite les commandes entrantes de l'utilisateur.
     */
    public string ProcessCommands(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Invalid command.");
        
        string[] parts = input.Trim().Split(' ', 2);
        string args = parts.Length > 1 ? parts[1] : string.Empty;
        string commandText = parts[0];
        
        try
        {
            CommandType commandType = CommandTypeUtils.FromStringToEnum(commandText);

            CommandBase command;

            switch (commandType)
            {
                case CommandType.Stocks:
                    command = new StocksCommand(_commandService, ParseRobotRequest(args));
                    break;
                case CommandType.NeededStocks:
                    command = new NeededStocksCommand(_commandService, ParseRobotRequest(args));
                    break;
                case CommandType.Instructions:
                    command = new InstructionsCommand(_commandService, ParseRobotRequest(args));
                    break;
                case CommandType.Verify:
                    command = new VerifyCommand(_commandService, ParseRobotRequest(args));
                    break;
                case CommandType.Produce:
                    command = new ProduceCommand(_commandService, ParseRobotRequest(args));
                    break;
                case CommandType.AddTemplate:
                    command = new AddTemplateCommand(_commandService, ParseTemplateRequest(args));
                    break;
                default:
                    throw new ArgumentException($"Unknown command '{commandText}'");
            }

            command.Execute();
            return _commandService.Output;
        }
        catch (ArgumentException e)
        {
            return  $"{SystemResponseTypeUtils.ToString(SystemResponseType.Error)} {e.Message}";
        }
    }
    
    /**
     * Analyse la chaîne d'arguments pour extraire les demandes de robots.
     */
    private Dictionary<RobotTemplate, int> ParseRobotRequest(string args)
    {
        Dictionary<RobotTemplate, int> requestedRobots = new Dictionary<RobotTemplate, int>();
        
        if (string.IsNullOrWhiteSpace(args))
            return requestedRobots;
        
        // Diviser les arguments par virgule
        string[] robotRequests = args.Split(',');
        
        foreach (string request in robotRequests)
        {
            string[] parts = request.Trim().Split(' ', 2);
            
            if (parts.Length != 2 || !int.TryParse(parts[0], out int quantity))
                throw new ArgumentException($"Invalid robot request format: {request}");
            
            string robotName = parts[1].Trim();

            RobotTemplate robotTemplate = _commandService.GetTemplate(robotName);
            
            // Ajoute ou met à jour la quantité demandée
            if (requestedRobots.ContainsKey(robotTemplate))
                requestedRobots[robotTemplate] += quantity;
            else
                requestedRobots[robotTemplate] = quantity;
        }
        
        return requestedRobots;
    }
    
    private Dictionary<RobotTemplate, int> ParseTemplateRequest(string args)
    {
        Dictionary<RobotTemplate, int> templateRequest = new Dictionary<RobotTemplate, int>();
        var parts = args.Split(',').Select(p => p.Trim()).ToArray();

        if (parts.Length != 6)
            throw new ArgumentException("ADD_TEMPLATE requires: Name, and 6 category parts");

        RobotTemplate newTemplate = new RobotTemplate(
            parts[0], // Name
            ECategory.Generalist, // category (volontairement généraliste pour être passée à l'assemblyman)
            RobotComponentResolver.ResolveName(parts[1]), // system
            RobotComponentResolver.ResolveName(parts[2]), // core
            RobotComponentResolver.ResolveName(parts[3]), // generator
            RobotComponentResolver.ResolveName(parts[4]), // arms
            RobotComponentResolver.ResolveName(parts[5]) // legs
            ); 
        
        templateRequest[newTemplate] = 1;
        return templateRequest;
    }
}
