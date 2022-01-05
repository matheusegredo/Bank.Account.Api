using Bank.Infrastructure.Cache.Models;
using Bank.Infrastructure.Cache.Models.AccountBalances;
using Microsoft.Extensions.Caching.Distributed;

namespace Bank.Infrastructure.Cache.Interfaces
{
    public class AccountBalanceCacheService : RedisCacheService<AccountBalanceCacheModel>, IAccountBalanceCacheService
    {
        public AccountBalanceCacheService(IDistributedCache distributedCache) : base(distributedCache) { }

        public override string KeyPrefix => "AccountBalance";

        public override int MinutesToExpire => 180;
    }
}
