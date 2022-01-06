using Bank.Infrastructure.Cache.Models.Authentication;

namespace Bank.Infrastructure.Cache.Interfaces
{
    public interface IAuthenticationJwtCacheService : IRedisCacheService<AuthenticationJwtCacheModel>
    {
    }
}
