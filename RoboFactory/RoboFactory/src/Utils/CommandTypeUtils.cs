namespace RoboFactory.Utils;

public static class CommandTypeUtils
{
    public static CommandType FromStringToEnum(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
            throw new ArgumentException("Command cannot be empty");

        return command.ToUpper() switch
        {
            "STOCKS" => CommandType.Stocks,
            "ADD_TEMPLATE" => CommandType.AddTemplate,
            "NEEDED_STOCKS" => CommandType.NeededStocks,
            "INSTRUCTIONS" => CommandType.Instructions,
            "VERIFY" => CommandType.Verify,
            "PRODUCE" => CommandType.Produce,
            "HELP" => CommandType.Help,
            "EXIT" => CommandType.Exit,
            _ => CommandType.Unknown
        };
    }
    
    
    public static string ToString(CommandType commandType)
    {
        return commandType switch
        {
            CommandType.Stocks => "STOCKS",
            CommandType.NeededStocks => "NEEDED_STOCKS",
            CommandType.Instructions => "INSTRUCTIONS",
            CommandType.Verify => "VERIFY",
            CommandType.Produce => "PRODUCE",
            CommandType.Help => "HELP",
            CommandType.Exit => "EXIT",
            CommandType.Unknown => "UNKNOWN",
            _ => throw new ArgumentOutOfRangeException(nameof(commandType))
        };
    }
}
