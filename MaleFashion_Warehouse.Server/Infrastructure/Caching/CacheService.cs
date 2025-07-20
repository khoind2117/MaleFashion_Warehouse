
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

            _cacheKeys.TryAdd(key, false);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.KeyDeleteAsync(_keyPrefix + key);

            _cacheKeys.TryRemove(key, out bool _);
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
            IEnumerable<Task> tasks = _cacheKeys
                .Keys
                .Where(k => k.StartsWith(_keyPrefix + prefix))
                .Select(k => RemoveAsync(k));
            
            await Task.WhenAll(tasks);
        }
    }
}
