using System;
using System.Threading.Tasks;

namespace StockSmart.Domain.Common.Abstract;

public interface IUnitOfWork : IDisposable
{
    Task<int> Complete();
}
