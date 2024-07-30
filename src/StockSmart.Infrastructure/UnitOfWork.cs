using System.Threading.Tasks;
using StockSmart.Domain.Common.Abstract;

namespace StockSmart.Infrastructure;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    private readonly AppDbContext _dbContext = dbContext;

    async Task<int> IUnitOfWork.Complete()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
