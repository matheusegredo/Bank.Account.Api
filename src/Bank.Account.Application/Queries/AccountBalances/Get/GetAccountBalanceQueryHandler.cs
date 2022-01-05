using Bank.Application.Commands.AccountBalances.Notifications.Post;
using Bank.CrossCutting.Exceptions;
using Bank.Data.Entities;
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

        public GetAccountBalanceQueryHandler(IMapper mapper, IMediator mediator, IBankContext bankContext, IAccountBalanceCacheService accountBalanceCacheService)
        {
            _mapper = mapper;
            _mediator = mediator;
            _bankContext = bankContext;
            _accountBalanceCacheService = accountBalanceCacheService;
        }

        public async Task<GetAccountBalanceQueryResponse> Handle(GetAccountBalanceQuery query, CancellationToken cancellationToken)
        {
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
