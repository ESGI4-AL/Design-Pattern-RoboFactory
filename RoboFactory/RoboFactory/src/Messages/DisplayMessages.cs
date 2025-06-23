namespace RoboFactory;

public static class DisplayMessages
{
    public static void DisplayWelcomeMessage()
    {
        Console.WriteLine("██████   ██████  ██████   ██████  ███████  █████   ██████ ████████  ██████  ██████  ██    ██");
        Console.WriteLine("██   ██ ██    ██ ██   ██ ██    ██ ██      ██   ██ ██         ██    ██    ██ ██   ██  ██  ██ ");
        Console.WriteLine("██████  ██    ██ ██████  ██    ██ █████   ███████ ██         ██    ██    ██ ██████    ████  ");
        Console.WriteLine("██   ██ ██    ██ ██   ██ ██    ██ ██      ██   ██ ██         ██    ██    ██ ██   ██    ██   ");
        Console.WriteLine("██   ██  ██████  ██████   ██████  ██      ██   ██  ██████    ██     ██████  ██   ██    ██   ");
        Console.WriteLine("");
        Console.WriteLine("Welcome to RoboFactory!");
        Console.WriteLine("");
        Console.WriteLine("To display the help menu, type 'HELP'");
        Console.WriteLine("To quit the program, type 'EXIT'");
        Console.WriteLine("=================================================");
        Console.WriteLine();
    }
    
    public static void DisplayHelpMenu()
    {
        Console.WriteLine("***********************************************************************");
        Console.WriteLine("*                              HELP MENU                              *");
        Console.WriteLine("***********************************************************************");
        Console.WriteLine("Available commands:");
        Console.WriteLine();
        Console.WriteLine("STOCKS");
        Console.WriteLine("    Displays all items currently in stock");
        Console.WriteLine();
        Console.WriteLine("NEEDED_STOCKS <quantity> <robot>, <quantity> <robot>, ...");
        Console.WriteLine("    Lists all parts needed to produce the specified robots");
        Console.WriteLine("    Example: NEEDED_STOCKS 2 XM-1, 1 RD-1");
        Console.WriteLine();
        Console.WriteLine("INSTRUCTIONS <quantity> <robot>, <quantity> <robot>, ...");
        Console.WriteLine("    Shows assembly instructions for the specified robots");
        Console.WriteLine("    Example: INSTRUCTIONS 1 XM-1");
        Console.WriteLine();
        Console.WriteLine("VERIFY <quantity> <robot>, <quantity> <robot>, ...");
        Console.WriteLine("    Checks if requested robots can be produced with current stock");
        Console.WriteLine("    Example: VERIFY 1 XM-1, 1 RD-1");
        Console.WriteLine();
        Console.WriteLine("PRODUCE <quantity> <robot>, <quantity> <robot>, ...");
        Console.WriteLine("    Produces the requested robots and updates the stock");
        Console.WriteLine("    Example: PRODUCE 1 XM-1");
        Console.WriteLine();
        Console.WriteLine("EXIT");
        Console.WriteLine("    Quits the program");
        Console.WriteLine("=================================================");
        Console.WriteLine();
    }
}
