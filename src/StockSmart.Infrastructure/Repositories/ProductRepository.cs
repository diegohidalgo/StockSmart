using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockSmart.Domain.Common.Abstract;
using StockSmart.Domain.Entities;
using StockSmart.Domain.Helpers;

namespace StockSmart.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly IStatusRepository _cachedStatusRepository;
        public ProductRepository(AppDbContext dbContext, IStatusRepository cachedStatusRepository) : base(dbContext)
        {
            this._cachedStatusRepository = cachedStatusRepository;
        }

        public async Task<Product> GetById(int id)
        {
            Guard.IsInvalidId(id);

            var product = await _appDbContext.Product.FindAsync(id);
            if (product is null)
            {
                return product;
            }
            var status = await _cachedStatusRepository.GetById(product.StatusId);
            if (status is null)
            {
                return product;
            }
            product.UpdateStatus(status);
            return product;
        }


        public async Task<Product> GetByIdWithIncludes(int id, params string[] includes)
        {
            Guard.IsInvalidId(id);

            IQueryable<Product> query = _appDbContext.Set<Product>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.SingleOrDefaultAsync(c => c.ProductId == id);
        }
    }
}
