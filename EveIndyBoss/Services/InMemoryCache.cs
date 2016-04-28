using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using EveIndyBoss.Models;

namespace EveIndyBoss.Services
{
    public interface ICacheThings
    {
        Task<CacheItem<T>> Get<T>(string key, Func<Task<T>> getter = null);
        Task<bool> Set<T>(string key, T data);
        Task Clear();
        Task<bool> Remove(string key);
    }

    public class InMemoryCache : ICacheThings
    {
        private readonly ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();

        public async Task<CacheItem<T>> Get<T>(string key, Func<Task<T>> getter = null)
        {
            if (!_cache.ContainsKey(key))
            {
                if(getter == null)
                    return CacheItem<T>.CreateNotFound();

                var retrieved = await getter();
                await Set(key, retrieved);

                return CacheItem<T>.CreateFound(retrieved);
            }

            object value;
            return _cache.TryGetValue(key, out value)
                ? CacheItem<T>.CreateFound((T) value)
                : CacheItem<T>.CreateNotFound();
        }

        public Task<bool> Set<T>(string key, T data)
        {
            return Task.FromResult(_cache.TryAdd(key, data));
        }

        public Task Clear()
        {
            _cache.Clear();
            return Task.FromResult(0);
        }

        public Task<bool> Remove(string key)
        {
            object value;
            return Task.FromResult(_cache.TryRemove(key, out value));
        }
    }
}