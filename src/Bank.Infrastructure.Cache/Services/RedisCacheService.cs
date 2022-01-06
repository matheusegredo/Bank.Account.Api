using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace Bank.Infrastructure.Cache.Models
{
    public abstract class RedisCacheService<TEntity>
    {
        public abstract string KeyPrefix { get; }

        public abstract int MinutesToExpire { get; }

        private readonly IDistributedCache _distributedCache;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public virtual bool TryGet(string key, out TEntity entity)
        {            
            var encodedObject = _distributedCache.Get(ResolveKey(key));

            if (encodedObject == null) 
            {
                entity = default;
                return false;
            }                

            entity = Decode(encodedObject);

            return true;
        }

        public virtual async Task<TEntity> Set(string key, TEntity entity, CancellationToken cancellationToken = default) 
        {
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(MinutesToExpire) };
            await _distributedCache.SetAsync(ResolveKey(key), Encode(entity), options, cancellationToken);
            return entity;
        }

        private string ResolveKey(string key) => $".{KeyPrefix}-{key}";

        private static TEntity Decode(byte[] bytes) 
        {
            var serializedObject = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<TEntity>(serializedObject);
        }

        private static byte[] Encode(TEntity entity)
        {
            var serializedObject = JsonConvert.SerializeObject(entity);
            var bytes = Encoding.UTF8.GetBytes(serializedObject);

            return bytes;
        }
        
    }
}
