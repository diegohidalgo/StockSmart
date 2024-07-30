using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockSmart.Domain.Common.Abstract;
using StockSmart.Domain.Entities;

namespace StockSmart.Infrastructure.Repositories;

public class StatusRepository(AppDbContext dbContext) : Repository<Status>(dbContext), IStatusRepository
{
    public async Task<Status> GetByIdWithIncludes(int id, params string[] includes)
    {
        IQueryable<Status> query = _appDbContext.Set<Status>();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.SingleOrDefaultAsync(c => c.StatusId == id);
    }

    public async Task<Status> GetByName(string name)
    {
        return _appDbContext.Status.Where(x => x.Name == name).FirstOrDefault();
    }
}
