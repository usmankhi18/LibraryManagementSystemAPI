using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Common.Cache
{
    public class RedisCacheManager
    {
        private readonly IDatabase _redisDb;

        public RedisCacheManager(string connectionString)
        {
           ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionString);
            _redisDb = redis.GetDatabase();
        }

        public void SaveDataToCache<T>(string key, T data, TimeSpan? expirationTime = null)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            byte[] serializedData = System.Text.Encoding.UTF8.GetBytes(jsonData);

            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();
            if (expirationTime.HasValue)
                cacheOptions.SetSlidingExpiration(expirationTime.Value);

            _redisDb.StringSet(key, serializedData, cacheOptions.AbsoluteExpirationRelativeToNow);
        }

        public T GetDataFromCache<T>(string key)
        {
            byte[] serializedData = _redisDb.StringGet(key);

            if (serializedData == null)
                return default(T);

            string jsonData = System.Text.Encoding.UTF8.GetString(serializedData);
            T data = JsonConvert.DeserializeObject<T>(jsonData);

            return data;
        }

        public void ClearCache(string key)
        {
            _redisDb.KeyDelete(key);
            Console.WriteLine("Cache cleared successfully.");
        }
    }
}
