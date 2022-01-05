using Bank.Infrastructure.Cache.Interfaces;
using Bank.Infrastructure.Cache.Models.AccountBalances;

namespace Bank.Application.Commands.AccountBalances.Notifications.Post
{
    public class PostAccountBalanceCacheHandler : INotificationHandler<PostCacheAccountBalanceNotification>
    {
        private readonly IMapper _mapper;
        private readonly IAccountBalanceCacheService _accountBalanceCacheService;

        public PostAccountBalanceCacheHandler(IMapper mapper, IAccountBalanceCacheService accountBalanceCacheService)
        {
            _mapper = mapper;
            _accountBalanceCacheService = accountBalanceCacheService;
        }

        public async Task Handle(PostCacheAccountBalanceNotification notification, CancellationToken cancellationToken)
        {
            await _accountBalanceCacheService.Set(notification.CacheKey, _mapper.Map<AccountBalanceCacheModel>(notification), cancellationToken);
        }
    }
}
