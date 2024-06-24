using Microsoft.Extensions.Caching.Memory;
using LazyCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Cache
{
    public class WrapperLazyCache : ICache
    {
        private readonly IAppCache _cache;
        public WrapperLazyCache(IAppCache cache)
        {
            _cache = cache;
        }
        public void Add<T>(string key, T item, TimeSpan expiration)
        {
            _cache.Add(key,item, expiration);
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiration)
        {
            return await _cache.GetOrAddAsync(key, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = expiration;
                return await factory();
            });
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
