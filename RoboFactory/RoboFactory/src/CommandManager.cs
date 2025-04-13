using RoboFactory.Utils;

namespace RoboFactory;

public class CommandManager
{
    private readonly FactoryInventory _inventory;
    private readonly AssemblyManager _assemblyManager;
    
    public CommandManager(FactoryInventory inventory)
    {
        _inventory = inventory;
        _assemblyManager = new AssemblyManager(_inventory);
    }
    
    /**
     * Traite les commandes entrantes de l'utilisateur.
     */
    public string ProcessCommands(string command)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(command))
                return $"{SystemResponseTypeUtils.ToString(SystemResponseType.Error)} Invalid command.";
            
            string[] parts = command.Trim().Split(' ', 2);
            string commandText = parts[0];
            string args = parts.Length > 1 ? parts[1] : string.Empty;
            
            CommandType commandType = CommandTypeUtils.FromStringToEnum(commandText);
            
            switch (commandType)
            {
                case CommandType.Stocks:
                    return _inventory.DisplayStock();
                case CommandType.NeededStocks:
                    return HandleNeededStocks(args);
                case CommandType.Instructions:
                    return HandleInstructions(args);
                case CommandType.Verify:
                    return HandleVerify(args);
                case CommandType.Produce:
                    return HandleProduce(args);
                case CommandType.Unknown:
                default:
                    return $"{SystemResponseTypeUtils.ToString(SystemResponseType.Error)} Unknown command '{commandText}'";
            }
        }
        catch (Exception e)
        {
            return $"{SystemResponseTypeUtils.ToString(SystemResponseType.Error)} {e.Message}";
        }
    }
    
    /**
     * Analyse la chaîne d'arguments pour extraire les demandes de robots.
     */
    private Dictionary<string, int> ParseRobotRequest(string args)
    {
        Dictionary<string, int> requestedRobots = new Dictionary<string, int>();
        
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
            
            // Vérifie si le robot existe
            if (!_inventory.IsValidRobot(robotName))
                throw new ArgumentException($"`{robotName}` is not a recognized robot");
            
            // Ajoute ou met à jour la quantité demandée
            if (requestedRobots.ContainsKey(robotName))
                requestedRobots[robotName] += quantity;
            else
                requestedRobots[robotName] = quantity;
        }
        
        return requestedRobots;
    }

    /**
     * Traite la commande "NeededStocks".
     */
    private string HandleNeededStocks(string args)
    {
        try
        {
            Dictionary<string, int> requestedRobots = ParseRobotRequest(args);
            
            if (requestedRobots.Count == 0)
                return $"{SystemResponseTypeUtils.ToString(SystemResponseType.Error)} No robot specified";
            
            return _assemblyManager.GenerateNeededStocks(requestedRobots);
        }
        catch (ArgumentException e)
        {
            return $"{SystemResponseTypeUtils.ToString(SystemResponseType.Error)} {e.Message}";
        }
    }

    /**
     * Traite la commande "Instructions".
     */
    private string HandleInstructions(string args)
    {
        try
        {
            Dictionary<string, int> requestedRobots = ParseRobotRequest(args);
            
            if (requestedRobots.Count == 0)
                return $"{SystemResponseTypeUtils.ToString(SystemResponseType.Error)} No robot specified";
            
            return _assemblyManager.GenerateAssemblyInstructions(requestedRobots);
        }
        catch (ArgumentException e)
        {
            return $"{SystemResponseTypeUtils.ToString(SystemResponseType.Error)} {e.Message}";
        }
    }

    /**
     * Traite la commande "Verify".
     */
    private string HandleVerify(string args)
    {
        try
        {
            Dictionary<string, int> requestedRobots = ParseRobotRequest(args);
            
            if (requestedRobots.Count == 0)
                return $"{SystemResponseTypeUtils.ToString(SystemResponseType.Error)} No robot specified";
            
            // Calcule les pièces nécessaires
            Dictionary<string, int> requiredPieces = _assemblyManager.CalculateRequiredPieces(requestedRobots);
            
            // Vérifie si les stocks sont suffisants
            bool hasStock = _inventory.HasSufficientStock(requiredPieces);
            
            return hasStock 
                ? SystemResponseTypeUtils.ToString(SystemResponseType.Available) 
                : SystemResponseTypeUtils.ToString(SystemResponseType.Unavailable);
        }
        catch (ArgumentException e)
        {
            return $"{SystemResponseTypeUtils.ToString(SystemResponseType.Error)} {e.Message}";
        }
    }

    /**
     * Traite la commande "Produce".
     */
    private string HandleProduce(string args)
    {
        try
        {
            Dictionary<string, int> requestedRobots = ParseRobotRequest(args);
        
            if (requestedRobots.Count == 0)
                return $"{SystemResponseTypeUtils.ToString(SystemResponseType.Error)} No robot specified";
        
            // Calcule les pièces nécessaires
            Dictionary<string, int> requiredPieces = _assemblyManager.CalculateRequiredPieces(requestedRobots);
        
            // Vérifie si les stocks sont suffisants
            bool hasStock = _inventory.HasSufficientStock(requiredPieces);
        
            if (!hasStock)
                return $"{SystemResponseTypeUtils.ToString(SystemResponseType.Error)} Insufficient stock for production";
        
            // Met à jour le stock (retire les pièces utilisées)
            _inventory.UpdateStock(requiredPieces, false);
        
            // Ajoute les robots produits au stock
            foreach (var robotRequest in requestedRobots)
            {
                _inventory.AddRobotToStock(robotRequest.Key, robotRequest.Value);
            }
        
            return SystemResponseTypeUtils.ToString(SystemResponseType.StockUpdated);
        }
        catch (ArgumentException e)
        {
            return $"{SystemResponseTypeUtils.ToString(SystemResponseType.Error)} {e.Message}";
        }
    }
}
