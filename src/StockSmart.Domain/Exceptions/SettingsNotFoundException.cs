using System.Net;

namespace StockSmart.Domain.Exceptions
{
    public class SettingsNotFoundException : StockSmartException
    {
        public SettingsNotFoundException(string message, object value) : base(message, value)
        {
        }
    }
}
