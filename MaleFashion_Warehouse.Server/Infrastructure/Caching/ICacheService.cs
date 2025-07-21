﻿namespace MaleFashion_Warehouse.Server.Infrastructure.Caching
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key) 
            where T : class;

        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
            where T : class;

        Task RemoveAsync(string key);

        Task RemoveByPrefixAsync(string prefix);

        Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> factory, TimeSpan? expiry = null)
            where T : class;
    }
}
