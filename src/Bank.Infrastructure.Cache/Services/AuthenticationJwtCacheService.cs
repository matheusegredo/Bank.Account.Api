using Bank.Infrastructure.Cache.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Bank.Infrastructure.Cache.Models.Authentication
{
    public class AuthenticationJwtCacheService : RedisCacheService<AuthenticationJwtCacheModel>, IAuthenticationJwtCacheService
    {
        public AuthenticationJwtCacheService(IDistributedCache distributedCache) : base(distributedCache) { }

        public override string KeyPrefix => ".Authentication-AccountNumber:";

        public override int MinutesToExpire => 60;
    }
}
