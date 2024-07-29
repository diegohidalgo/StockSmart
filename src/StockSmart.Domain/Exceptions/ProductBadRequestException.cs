using System.Net;

namespace StockSmart.Domain.Exceptions
{
    public class ProductBadRequestException : StockSmartException
    {
        public ProductBadRequestException(string message, object value) : base(message, value)
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}
