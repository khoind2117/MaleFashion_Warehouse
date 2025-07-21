
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MaleFashion_Warehouse.Server.Infrastructure.Caching
{
    public class CacheService : ICacheService
    {
        private static readonly ConcurrentDictionary<string, bool> _cacheKeys = new();
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();
        private static readonly MemoryCache _lockCache = new(new MemoryCacheOptions());

        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _cache;

        /// Prefix to namespace cache keys and avoid key collisions across projects.
        private readonly string _keyPrefix = "mfw-cache:";
        
        public CacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _cache = _connectionMultiplexer.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            var value = await _cache.StringGetAsync(_keyPrefix + key);
            return value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(value);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null) where T : class
        {
            var json = JsonSerializer.Serialize(value);
            await _cache.StringSetAsync(_keyPrefix + key, json, expiry);
                
            _cacheKeys.TryAdd(_keyPrefix + key, false);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.KeyDeleteAsync(_keyPrefix + key);
                
            _cacheKeys.TryRemove(_keyPrefix + key, out _);
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
            IEnumerable<Task> tasks = _cacheKeys
                .Keys
                .Where(k => k.StartsWith(_keyPrefix + prefix))
                .Select(k => RemoveAsync(k));
            
            await Task.WhenAll(tasks);
        }

        public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null)
            where T : class
        {
            var cacheKey = _keyPrefix + key;

            var cachedValue = await _cache.StringGetAsync(cacheKey);

            T? value;
            if (!string.IsNullOrWhiteSpace(cachedValue)) 
            {
                value = JsonSerializer.Deserialize<T>(cachedValue);

                if (value != null)
                {
                    return value;
                }
            }

            var myLock = _lockCache.GetOrCreate(cacheKey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(5);
                return new SemaphoreSlim(1, 1);
            });

            var hasLock = await myLock.WaitAsync(500);
            if (!hasLock)
            {
                return default;
            }

            try
            {
                cachedValue = await _cache.StringGetAsync(cacheKey);
                if (!string.IsNullOrWhiteSpace(cachedValue))
                {
                    value = JsonSerializer.Deserialize<T>(cachedValue);

                    if (value != null)
                    {
                        return value;
                    }
                }

                value = await factory();
                if (value != null)
                {
                    var json = JsonSerializer.Serialize(value);
                    await _cache.StringSetAsync(cacheKey, json, expiry ?? TimeSpan.FromMinutes(2));
                    _cacheKeys.TryAdd(cacheKey, false);
                }

                return value;
            }
            finally
            {
                myLock.Release();

                if (myLock.CurrentCount == 1)
                {
                    _locks.TryRemove(cacheKey, out _);
                }
            }
        }
    }
}
