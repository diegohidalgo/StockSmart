using StockSmart.Domain.Exceptions;

namespace StockSmart.Domain.Helpers
{
    public class Guard
    {
        public static void IsInvalidId(int? id)
        {
            if (id is null || id < 1)
            {
                throw new ProductBadRequestException("Error sending id parameter", id);
            }
        }
    }
}
