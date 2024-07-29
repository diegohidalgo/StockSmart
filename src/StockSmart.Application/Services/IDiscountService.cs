using System.Collections.Generic;
using System.Threading.Tasks;
using StockSmart.Application.Dto;

namespace StockSmart.Application.Services
{
    public interface IDiscountService
    {
        Task<IEnumerable<DiscountDto>> GetDiscountByProducts(IEnumerable<string> productCodes);
    }
}
