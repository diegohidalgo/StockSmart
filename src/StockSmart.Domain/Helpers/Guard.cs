using StockSmart.Domain.Exceptions;

namespace StockSmart.Domain.Helpers;

public class Guard
{
    public static void IsInvalidId(int? id)
    {
        if (id is null or < 1)
        {
            throw new ProductBadRequestException("Error sending id parameter", id);
        }
    }
}
