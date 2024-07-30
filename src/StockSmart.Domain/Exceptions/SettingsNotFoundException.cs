namespace StockSmart.Domain.Exceptions;

public class SettingsNotFoundException(string message, object value) : StockSmartException(message, value)
{
}
