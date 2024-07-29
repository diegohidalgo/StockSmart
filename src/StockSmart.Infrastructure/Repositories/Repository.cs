using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockSmart.Domain.Common.Abstract;

namespace StockSmart.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _appDbContext;

        public Repository(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        public async Task Add(TEntity values)
        {
            await _appDbContext.Set<TEntity>().AddAsync(values);
        }
        public async Task AddRange(IEnumerable<TEntity> values)
        {
            await _appDbContext.Set<TEntity>().AddRangeAsync(values);
        }
        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await _appDbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _appDbContext.Set<TEntity>().ToListAsync();
        }
        public async Task<TEntity> GetById(int id)
        {
            return await _appDbContext.Set<TEntity>().FindAsync(id);
        }

        public Task Remove(IEnumerable<TEntity> values)
        {
            _appDbContext.Set<TEntity>().RemoveRange(values);
            return Task.CompletedTask;
        }
    }
}
