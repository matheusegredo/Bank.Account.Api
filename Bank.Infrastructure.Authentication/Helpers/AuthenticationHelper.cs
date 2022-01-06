using Bank.CrossCutting.Exceptions;
using Bank.Data.Entities;
using Bank.Infrastructure.Authentication.Interfaces;
using Bank.Infrastructure.Cache.Interfaces;
using Bank.Infrastructure.Cache.Models.Authentication;
using Microsoft.AspNetCore.Http;

namespace Bank.Infrastructure.Authentication.Helpers
{
    public class AuthenticationHelper : IAuthenticationHelper
    {        
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthenticationJwtCacheService _authenticationJwtCacheService;

        public AuthenticationHelper(IHttpContextAccessor httpContextAccessor, IAuthenticationJwtCacheService authenticationJwtCacheService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authenticationJwtCacheService = authenticationJwtCacheService;
        }

        public void ValidateTokenByAccount(Account account, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(account.AccountNumber))
                throw new ArgumentException("Account number cannot be null or white space.");

            if (!_authenticationJwtCacheService.TryGet(account.AccountNumber, out AuthenticationJwtCacheModel authentication))
                throw new UnauthorizedAccessException($"Account number {account.AccountNumber} is unauthorized, please make the authentication again");

            if (authentication.Token != GetToken())
                throw new ForbidenRequestException($"The token don't belongs to account number {account.AccountNumber}");
        }

        private string GetToken()
        {
            if (!_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
                return string.Empty;

            return _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
        }
    }
}
