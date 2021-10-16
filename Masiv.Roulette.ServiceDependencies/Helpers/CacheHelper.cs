using Masiv.Roulette.Service;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Masiv.Roulette.ServiceDependencies
{
    public class CacheHelper : ICacheHelper
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IMemoryCache _cache;

        public CacheHelper(IDistributedCache distributedCache, IMemoryCache cache)
        {
            _distributedCache = distributedCache;
            _cache = cache;
        }

        public async Task AddCache<T>(T value, string key, string dependency)
        {
            string taskKey = GetKey<T>(key: key);
            await _distributedCache.SetAsync(key: taskKey, ToByteArray(value));
            _cache.Set(key: taskKey, value);
            await DeleteCache<T>(dependency);
        }

        public async Task AddListCache<T>(List<T> value, string key)
        {
            string taskKey = GetListKey<T>(key: key);
            await _distributedCache.SetAsync(key: taskKey, ToByteArray(value));
            _cache.Set(key: taskKey, value);
        }

        private async Task DeleteCache<T>(string key)
        {
            string taskKey = GetListKey<T>(key: key);
            await _distributedCache.RemoveAsync(taskKey);
            _cache.Remove(taskKey);
        }

        public async Task<T> ResolveCache<T>(string key)
        {
            string taskKey = GetKey<T>(key: key);
            if (!_cache.TryGetValue(key: taskKey, out T entity))
            {
                byte[] bytes = await _distributedCache.GetAsync(key: taskKey);
                if (bytes != null)
                {
                    entity = FromByteArray<T>(bytes);
                    _cache.Set(key: taskKey, entity);
                }
            }

            return entity;
        }

        public async Task<List<T>> ResolveListCache<T>(string key)
        {
            string taskKey = GetListKey<T>(key: key);
            if (!_cache.TryGetValue(key: taskKey, out List<T> entities))
            {
                byte[] bytes = await _distributedCache.GetAsync(key: taskKey);
                if (bytes != null)
                {
                    entities = FromByteArray<List<T>>(bytes);
                    _cache.Set(key: taskKey, entities);
                }
            }

            return entities;
        }

        private static string GetKey<T>(string key)
        {
            return $"{typeof(T).Name}-{key}";
        }

        private static string GetListKey<T>(string key)
        {
            return $"{typeof(List<T>).Name}{typeof(T).Name}-{key}";
        }

        private static T FromByteArray<T>(byte[] bytes)
        {
            return JsonSerializer.Deserialize<T>(bytes);
        }

        private static byte[] ToByteArray<T>(T value)
        {
            return JsonSerializer.SerializeToUtf8Bytes(value);
        }
    }
}
