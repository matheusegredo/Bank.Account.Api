using Bank.Infrastructure.Cache.Models.AccountBalances;

namespace Bank.Infrastructure.Cache.Interfaces
{
    public interface IAccountBalanceCacheService : IRedisCacheService<AccountBalanceCacheModel>
    {
    }
}
