using System.Net;

namespace StockSmart.Domain.Exceptions;

public class ProductStatusInvalidException : StockSmartException
{
    public ProductStatusInvalidException(string message, object value) : base(message, value)
    {
        StatusCode = (int)HttpStatusCode.BadRequest;
    }
}
