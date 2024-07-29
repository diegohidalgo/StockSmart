using System.Threading.Tasks;
using StockSmart.Domain.Entities;

namespace StockSmart.Domain.Common.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetByIdWithIncludes(int id, params string[] includes);

    }
}
