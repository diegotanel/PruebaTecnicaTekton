using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Cache
{
    public interface ICache
    {
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiration);
        void Remove(string key);
        void Add<T>(string key, T item, TimeSpan expiration);
    }
}
