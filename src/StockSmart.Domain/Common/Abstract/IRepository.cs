using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace StockSmart.Domain.Common.Abstract;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetById(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);

    Task Add(TEntity values);
    Task AddRange(IEnumerable<TEntity> values);
    Task Remove(IEnumerable<TEntity> values);
}
