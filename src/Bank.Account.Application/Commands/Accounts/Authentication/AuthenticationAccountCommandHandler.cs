using Bank.Infrastructure.Authentication;
using Bank.Infrastructure.Cache.Interfaces;
using Bank.Infrastructure.Cache.Models.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Commands.Accounts.Authentication
{
    public class AuthenticationAccountCommandHandler : IRequestHandler<AuthenticationAccountCommand, AuthenticationAccountCommandResponse>
    {
        private readonly IBankContext _bankContext;
        private readonly IAuthenticationJwtCacheService _authenticationJwtCacheService;
        public AuthenticationAccountCommandHandler(IBankContext bankContext, IAuthenticationJwtCacheService authenticationJwtCacheService)
        {
            _bankContext = bankContext;
            _authenticationJwtCacheService = authenticationJwtCacheService;
        }

        public async Task<AuthenticationAccountCommandResponse> Handle(AuthenticationAccountCommand command, CancellationToken cancellationToken)
        {
            var account = await _bankContext.Accounts
                .Where(account => account.AccountNumber == command.AccountNumber)                
                .FirstOrDefaultAsync(cancellationToken);

            if (account is null)
                throw new UnauthorizedAccessException("Authentication failed.");

            var tokenIdentifier = Guid.NewGuid().ToString();

            var expiresOn = DateTime.UtcNow.AddMinutes(60);
            var token = new JwtTokenGenerator().GenerateToken(account.AccountNumber, expiresOn);
            await _authenticationJwtCacheService.Set(account.AccountNumber, new AuthenticationJwtCacheModel(token, tokenIdentifier), cancellationToken);

            return new AuthenticationAccountCommandResponse(token, expiresOn);
        }
    }
}
