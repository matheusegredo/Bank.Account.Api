using Bank.Application.Commands.AccountBalances.Notifications.Post;
using Bank.Application.Helpers;
using Bank.CrossCutting.Exceptions;
using Bank.Data.Entities;
using Bank.Infrastructure.Authentication.Interfaces;
using Bank.Infrastructure.Cache.Interfaces;
using Bank.Infrastructure.Cache.Models.AccountBalances;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Queries.AccountBalances
{
    public class GetAccountBalanceQueryHandler : IRequestHandler<GetAccountBalanceQuery, GetAccountBalanceQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IBankContext _bankContext;
        private readonly IAccountBalanceCacheService _accountBalanceCacheService;
        private readonly IAuthenticationHelper _authenticationHelper;

        public GetAccountBalanceQueryHandler(IMapper mapper,
            IMediator mediator, 
            IBankContext bankContext,
            IAccountBalanceCacheService accountBalanceCacheService,
            IAuthenticationHelper authenticationHelper)
        {
            _mapper = mapper;
            _mediator = mediator;
            _bankContext = bankContext;
            _accountBalanceCacheService = accountBalanceCacheService;
            _authenticationHelper = authenticationHelper;
        }

        public async Task<GetAccountBalanceQueryResponse> Handle(GetAccountBalanceQuery query, CancellationToken cancellationToken)
        {
            var account = await _bankContext.Accounts
                .Where(account => account.AccountId == query.AccountId)
                .FirstOrDefaultReadingUncomittedAsync(cancellationToken);

            if (account is null)
                throw new NotFoundException("Account not found.");

            _authenticationHelper.ValidateTokenByAccount(account, cancellationToken);

            if (_accountBalanceCacheService.TryGet($"AccountId:{query.AccountId}", out AccountBalanceCacheModel accountBalanceCache))            
                return _mapper.Map<GetAccountBalanceQueryResponse>(accountBalanceCache);

            var accountBalance = await _bankContext.AccountBalances
                .Where(accountBalance => accountBalance.AccountId == query.AccountId)
                .Select(accountBalance => new AccountBalance { AccountId = accountBalance.AccountId, Value = accountBalance.Value })
                .FirstOrDefaultAsync(cancellationToken);

            if (accountBalance is null)
                throw new NotFoundException("Account balance not found.");

            await _mediator.Publish(new PostCacheAccountBalanceNotification(accountBalance.AccountId, accountBalance.Value), cancellationToken);

            return _mapper.Map<GetAccountBalanceQueryResponse>(accountBalance);
        }
    }
}
