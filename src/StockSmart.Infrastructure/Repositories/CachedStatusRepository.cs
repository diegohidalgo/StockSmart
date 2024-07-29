using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StockSmart.Domain.Common.Abstract;
using StockSmart.Domain.Entities;

namespace StockSmart.Infrastructure.Repositories
{
    public class CachedStatusRepository : Repository<Status>, IStatusRepository
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IDistributedCache _cache;
        public CachedStatusRepository(AppDbContext dbContext, IStatusRepository statusRepository, IDistributedCache cache) : base(dbContext)
        {
            _statusRepository = statusRepository;
            _cache = cache;
        }

        public async Task<Status> GetById(int id)
        {
            var statusKey = $"status-{id}";
            var cachedStatus = await _cache.GetStringAsync(statusKey);

            if (!string.IsNullOrEmpty(cachedStatus))
            {
                return JsonConvert.DeserializeObject<Status>(cachedStatus,
                                            new JsonSerializerSettings
                                            {
                                                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor

                                            });
            }

            var status = await _statusRepository.GetById(id);
            if (status == null)
                return status;

            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));
            _cache.SetString(statusKey, JsonConvert.SerializeObject(status), options);

            return status;
        }

        public async Task<Status> GetByName(string name)
        {
            var statusKey = $"status-name-{name}";
            var cachedStatus = await _cache.GetStringAsync(statusKey);

            if (!string.IsNullOrEmpty(cachedStatus))
            {
                return JsonConvert.DeserializeObject<Status>(cachedStatus,
                                            new JsonSerializerSettings
                                            {
                                                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor

                                            });
            }

            var status = (await _statusRepository.Find(x => x.Name == name)).FirstOrDefault();

            if (status == null)
                return status;

            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));
            _cache.SetString(statusKey, JsonConvert.SerializeObject(status), options);

            return status;
        }
    }
}
