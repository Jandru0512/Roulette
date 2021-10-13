using Masiv.Roulette.Service;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
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

        public async Task AddCache<T>(object key, T value)
        {
            string taskKey = GetKey<T>(key: key);
            await _distributedCache.SetAsync(key: taskKey, ToByteArray(value));
            _cache.Set(key: taskKey, value);
        }

        public async Task<T> ResolveChace<T>(object key)
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

        private static string GetKey<T>(object key)
        {
            return $"{typeof(T).Name}-{key}";
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
