using System.Text;

namespace RoboFactory;

public class FactoryInventory
{
    private readonly Dictionary<string, RobotComponent> _pieces;
    private readonly Dictionary<string, Robot> _robots;

    public FactoryInventory()
    {
        _pieces = new Dictionary<string, RobotComponent>();
        _robots = new Dictionary<string, Robot>();
        InitializeInventory();
    }
    
    /**
     * Ajout des pièces de robots et des robots entiers à l'inventaire
     */
    private void InitializeInventory()
    {
        // Modules principaux
        AddPiece(new RobotComponent("Core_CM1", "CoreModule", 10, true));
        AddPiece(new RobotComponent("Core_CD1", "CoreModule", 10, true));
        AddPiece(new RobotComponent("Core_CI1", "CoreModule", 10, true));
        
        // Générateurs
        AddPiece(new RobotComponent("Generator_GM1", "Generator", 10));
        AddPiece(new RobotComponent("Generator_GD1", "Generator", 10));
        AddPiece(new RobotComponent("Generator_GI1", "Generator", 10));
        
        // Modules de préhension
        AddPiece(new RobotComponent("Arms_AM1", "GraspingModule", 10));
        AddPiece(new RobotComponent("Arms_AD1", "GraspingModule", 10));
        AddPiece(new RobotComponent("Arms_AI1", "GraspingModule", 10));
        
        // Modules de déplacement
        AddPiece(new RobotComponent("Legs_LM1", "MovementModule", 10));
        AddPiece(new RobotComponent("Legs_LD1", "MovementModule", 10));
        AddPiece(new RobotComponent("Legs_LI1", "MovementModule", 10));
        
        // Système
        AddPiece(new RobotComponent("System_SB1", "System", 10));
        
        // Ajout des robots
        AddRobot(new Robot("XM-1", 2, "Core_CM1", "Generator_GM1", "Arms_AM1", "Legs_LM1", "System_SB1"));
        AddRobot(new Robot("RD-1", 1, "Core_CD1", "Generator_GD1", "Arms_AD1", "Legs_LD1", "System_SB1"));
        AddRobot(new Robot("WI-1", 3, "Core_CI1", "Generator_GI1", "Arms_AI1", "Legs_LI1", "System_SB1"));
    }

    /**
     * Ajoute une pièce de robot à l'inventaire ou met à jour sa quantité si elle existe déjà.
     */
    private void AddPiece(RobotComponent robotComponent)
    {
        if (_pieces.TryGetValue(robotComponent.Name, out var existingComponent))
        {
            existingComponent.Quantity += robotComponent.Quantity;
        }
        else
        {
            _pieces.Add(robotComponent.Name, robotComponent);
        }
    }

    /**
     * Ajoute un robot à l'inventaire ou met à jour sa quantité si il existe déjà.
     */
    private void AddRobot(Robot robot)
    {
        if (_robots.TryGetValue(robot.Name, out var existingRobot))
        {
            existingRobot.Quantity += robot.Quantity;
        }
        else
        {
            _robots.Add(robot.Name, robot);
        }
    }
    
    /**
     * Vérifie si un robot est valide en fonction de son nom.
     */
    public bool IsValidRobot(string robotName)
    {
        return _robots.ContainsKey(robotName);
    }
    
    /**
     * Récupère un robot en fonction de son nom.
     */
    public Robot? GetRobot(string robotName)
    {
        return _robots.GetValueOrDefault(robotName);
    }
    
    /**
     * Récupère une pièce de robot en fonction de son nom.
     */
    public RobotComponent? GetPiece(string pieceName)
    {
        return _pieces.GetValueOrDefault(pieceName);
    }
    
    /**
     * Affiche les pièces et robots en stock.
     */
    public string DisplayStock()
    {
        var result = new StringBuilder();
        
        // Affiche les robots en stock
        foreach (var robot in _robots.Values)
        {
            result.AppendLine($"{robot.Quantity} {robot.Name}");
        }
        
        // Affiche les pièces en stock
        foreach (var piece in _pieces.Values)
        {
            result.AppendLine($"{piece.Quantity} {piece.Name}");
        }
        
        return result.ToString();
    }
    
    /**
     * Verifie si l'inventaire a suffisamment de stock pour assembler les robots demandés.
     */
    public bool HasSufficientStock(Dictionary<string, int> requiredParts)
    {
        foreach (var part in requiredParts)
        {
            if (!_pieces.TryGetValue(part.Key, out var piece) || piece.Quantity < part.Value)
                return false;
        }
        return true;
    }
    
    /**
     * Met à jour le stock en ajoutant ou retirant des pièces.
     */
    public void UpdateStock(Dictionary<string, int> parts, bool add)
    {
        foreach (var part in parts)
        {
            if (_pieces.TryGetValue(part.Key, out var piece))
            {
                piece.Quantity = add ? piece.Quantity + part.Value : piece.Quantity - part.Value;
            }
            else if (add)
            {
                AddPiece(new RobotComponent(part.Key, "Unknown", part.Value));
            }
        }
    }
    
    /**
     * Ajoute un robot au stock de l'inventaire.
     * D'abord on vérifie si le robot existe déjà dans l'inventaire.
     * S'il existe, on met à jour la quantité.
     * Sinon, on crée un nouveau robot avec les mêmes caractéristiques que le modèle.
     */
    public void AddRobotToStock(string robotName, int quantity)
    {
        if (_robots.TryGetValue(robotName, out var existingRobot))
        {
            existingRobot.Quantity += quantity;
        }
        else
        {
            Robot? template = null;
            foreach (var robot in _robots.Values)
            {
                if (robot.Name.Equals(robotName))
                {
                    template = robot;
                    break;
                }
            }

            if (template == null) return;
            
            var newRobot = new Robot(robotName, quantity, 
                template.CoreName, template.GeneratorName, 
                template.ArmsName, template.LegsName, template.SystemName);
            _robots.Add(robotName, newRobot);
        }
    }
}
