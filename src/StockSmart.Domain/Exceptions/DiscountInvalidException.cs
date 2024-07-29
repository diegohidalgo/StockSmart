using System.Net;

namespace StockSmart.Domain.Exceptions
{
    public class DiscountInvalidException : StockSmartException
    {
        public DiscountInvalidException(string message, object value) : base(message, value)
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}
