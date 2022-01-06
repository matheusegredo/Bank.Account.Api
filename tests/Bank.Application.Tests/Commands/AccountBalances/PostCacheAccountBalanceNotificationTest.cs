using AutoMapper;
using Bank.Application.Commands.AccountBalances.Notifications.Post;
using Bank.Application.Profiles;
using Bank.Infrastructure.Cache.Interfaces;
using Bank.Infrastructure.Cache.Models.AccountBalances;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Xunit;

namespace Bank.Application.Tests.Commands.AccountBalances
{
    [Trait("Category", "AccountBalance")]
    public class PostCacheAccountBalanceNotificationTest
    {
        private readonly IAccountBalanceCacheService _accountBalanceCacheService;
        private readonly PostAccountBalanceCacheHandler _handler;

        public PostCacheAccountBalanceNotificationTest()
        {
            var cacheOptions = Options.Create(new MemoryDistributedCacheOptions());
            var cache = new MemoryDistributedCache(cacheOptions);

            _accountBalanceCacheService = new AccountBalanceCacheService(cache);
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AccountBalanceProfile())).CreateMapper();

            _handler = new PostAccountBalanceCacheHandler(mapper, _accountBalanceCacheService);
        }

        [Fact(DisplayName = "Sending valid contract should set cache value")]
        public async Task SendingValidContract_ShouldSetValue()
        {
            var notification = new PostCacheAccountBalanceNotification(1, 10);
            await _handler.Handle(notification, default);

            Assert.True(_accountBalanceCacheService.TryGet(notification.CacheKey, out AccountBalanceCacheModel accountBalance));
            Assert.Equal(accountBalance.Value, notification.Value);
        }
    }
}
