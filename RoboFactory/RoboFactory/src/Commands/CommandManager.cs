using RoboFactory.Commands.CommandTypes;
using RoboFactory.Utils;

namespace RoboFactory.Commands;

public class CommandManager
{
    private readonly InventoryCommandService _commandService;
    
    public CommandManager(InventoryCommandService commandService)
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
            Dictionary<(ECategory, EItemType), int> requestedRobots = ParseRobotRequest(args);

            CommandType commandType = CommandTypeUtils.FromStringToEnum(commandText);

            CommandBase command;

            switch (commandType)
            {
                case CommandType.Stocks:
                    command = new StocksCommand(_commandService, requestedRobots);
                    break;
                case CommandType.NeededStocks:
                    command = new NeededStocksCommand(_commandService, requestedRobots);
                    break;
                case CommandType.Instructions:
                    command = new InstructionsCommand(_commandService, requestedRobots);
                    break;
                case CommandType.Verify:
                    command = new VerifyCommand(_commandService, requestedRobots);
                    break;
                case CommandType.Produce:
                    command = new ProduceCommand(_commandService, requestedRobots);
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
    private Dictionary<(ECategory, EItemType), int> ParseRobotRequest(string args)
    {
        Dictionary<(ECategory, EItemType), int> requestedRobots = new Dictionary<(ECategory, EItemType), int>();
        
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
            
            // Résolve la catégorie du robot et vérifie s'il existe
            (ECategory category, EItemType itemType) = RobotResolver.ResolveName(robotName);
            
            // Ajoute ou met à jour la quantité demandée
            if (requestedRobots.ContainsKey((category, itemType)))
                requestedRobots[(category, itemType)] += quantity;
            else
                requestedRobots[(category, itemType)] = quantity;
        }
        
        return requestedRobots;
    }
}
