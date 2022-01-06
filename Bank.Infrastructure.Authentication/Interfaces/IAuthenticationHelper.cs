using Bank.Data.Entities;

namespace Bank.Infrastructure.Authentication.Interfaces
{
    public interface IAuthenticationHelper
    {
        void ValidateTokenByAccount(Account account, CancellationToken cancellationToken = default);
    }
}
