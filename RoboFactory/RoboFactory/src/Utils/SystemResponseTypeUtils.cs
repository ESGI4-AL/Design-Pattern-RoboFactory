namespace RoboFactory.Utils;

public static class SystemResponseTypeUtils
{
    public static SystemResponseType FromStringToEnum(string systemResponse)
    {
        if (string.IsNullOrWhiteSpace(systemResponse))
            throw new ArgumentException("System response cannot be empty");

        return systemResponse.ToUpper() switch
        {
            "AVAILABLE" => SystemResponseType.Available,
            "UNAVAILABLE" => SystemResponseType.Unavailable,
            "STOCK_UPDATED" => SystemResponseType.StockUpdated,
            "ERROR" => SystemResponseType.Error,
            _ => SystemResponseType.Error
        };
    }
    
    public static string ToString(SystemResponseType systemResponseType)
    {
        return systemResponseType switch
        {
            SystemResponseType.Available => "AVAILABLE",
            SystemResponseType.Unavailable => "UNAVAILABLE",
            SystemResponseType.StockUpdated => "STOCK_UPDATED",
            SystemResponseType.TemplatesUpdated => "TEMPLATE_UPDATED",
            SystemResponseType.Error => "ERROR",
            _ => throw new ArgumentOutOfRangeException(nameof(systemResponseType))
        };
    }
}
