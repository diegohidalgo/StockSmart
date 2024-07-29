using System.Threading.Tasks;
using StockSmart.Domain.Common.Abstract;
using StockSmart.Infrastructure.Repositories;

namespace StockSmart.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        async Task<int> IUnitOfWork.Complete()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
