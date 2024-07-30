using System.Net;

namespace StockSmart.Domain.Exceptions;

public class ProductNotFoundException : StockSmartException
{
    public ProductNotFoundException(string message, object value) : base(message, value)
    {
        StatusCode = (int)HttpStatusCode.NotFound;
    }
}
