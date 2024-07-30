using System.Net;

namespace StockSmart.Domain.Exceptions;

public class StatusNotFoundException : StockSmartException
{
    public StatusNotFoundException(string message, object value) : base(message, value)
    {
        StatusCode = (int)HttpStatusCode.BadRequest;
    }
}
