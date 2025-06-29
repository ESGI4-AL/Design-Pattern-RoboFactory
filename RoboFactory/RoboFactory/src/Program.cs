using RoboFactory.Commands;
using RoboFactory.Utils;

namespace RoboFactory
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var factoryInventory = new FactoryInventory();
            var assemblyManual = new AssemblyManual();
            var commandService = new CommandService(factoryInventory, assemblyManual);
            var commandManager = new CommandManager(commandService);
            
            DisplayMessages.DisplayWelcomeMessage();
            Console.WriteLine("Enter your command :");

            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();
                
                if (string.IsNullOrEmpty(input))
                    continue;

                string command = input.Split(' ')[0];
                CommandType commandType = CommandTypeUtils.FromStringToEnum(command);
                
                if (commandType == CommandType.Exit)
                {
                    Console.WriteLine("Exiting the program...");
                    break;
                }
                
                if (commandType == CommandType.Help)
                {
                    DisplayMessages.DisplayHelpMenu();
                    continue;
                }
                
                string result = commandManager.ProcessCommands(input);
                Console.WriteLine(result);
            }
            
            Console.WriteLine("Good bye!");
        }
    }
}
