
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Text.Json;

namespace MaleFashion_Warehouse.Server.Infrastructure.Caching
{
    public class CacheService : ICacheService
    {
        private static readonly ConcurrentDictionary<string, bool> _cacheKeys = new();

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
            try
            {
                var value = await _cache.StringGetAsync(_keyPrefix + key);
                return value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(value);
            }
            catch
            {
                return default; // Fail silently if Redis is unavailable
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null) where T : class
        {
            try
            {
                var json = JsonSerializer.Serialize(value);
                await _cache.StringSetAsync(_keyPrefix + key, json, expiry);
                
                _cacheKeys.TryAdd(_keyPrefix + key, false);
            }
            catch
            {
                // Fail silently if Redis is unavailable
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _cache.KeyDeleteAsync(_keyPrefix + key);
                
                _cacheKeys.TryRemove(_keyPrefix + key, out _);
            }
            catch
            {
                // Fail silently if Redis is unavailable
            }
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
            try
            {
                    IEnumerable<Task> tasks = _cacheKeys
                        .Keys
                        .Where(k => k.StartsWith(_keyPrefix + prefix))
                        .Select(k => RemoveAsync(k));
            
                    await Task.WhenAll(tasks);
                }
            catch 
            {
                // Fail silently if Redis is unavailable
            }
        }
    }
}
