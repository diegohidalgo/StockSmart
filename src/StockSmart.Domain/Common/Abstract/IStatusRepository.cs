using System.Threading.Tasks;
using StockSmart.Domain.Entities;

namespace StockSmart.Domain.Common.Abstract;

public interface IStatusRepository : IRepository<Status>
{
    Task<Status> GetByName(string name);
}
