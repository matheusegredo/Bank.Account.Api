using Bank.Application.Commands.AccountBalances.Notifications.Post;
using Bank.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Commands.AccountBalances.Notifications
{
    public class PutAccountBalanceNotificationHandler : INotificationHandler<PutAccountBalanceNotification>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;        
        private readonly IBankContext _bankContext;

        public PutAccountBalanceNotificationHandler(IMapper mapper, IMediator mediator, IBankContext bankContext)
        {
            _mapper = mapper;
            _mediator = mediator;
            _bankContext = bankContext;
        }

        public async Task Handle(PutAccountBalanceNotification notification, CancellationToken cancellationToken)
        {
            var accountBalance = await _bankContext.AccountBalances
                .Where(accountBalance => accountBalance.AccountId == notification.AccountId)
                .FirstOrDefaultAsync(cancellationToken);

            if (accountBalance is null) 
            {
                accountBalance = _mapper.Map<AccountBalance>(notification);
                _bankContext.AccountBalances.Add(accountBalance);
            }
            else             
                accountBalance.Value += notification.Value;

            accountBalance.LastTimeChanged = DateTime.UtcNow;

            await _bankContext.SaveChangesAsync(cancellationToken);
            await _mediator.Publish(new PostCacheAccountBalanceNotification(accountBalance.AccountId, accountBalance.Value), cancellationToken);
        }
    }
}
