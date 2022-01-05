using AutoMapper;
using Bank.Application.Commands.AccountBalances.Notifications;
using Bank.Application.Commands.AccountBalances.Notifications.Post;
using Bank.Application.Profiles;
using Bank.Application.Queries.AccountBalances;
using Bank.Application.Tests.Moq;
using Bank.CrossCutting.Exceptions;
using Bank.Data.Entities;
using Bank.Infrastructure.Cache.Interfaces;
using Bank.Infrastructure.Cache.Models.AccountBalances;
using Bank.Persistence.Interfaces;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bank.Application.Tests.Commands.AccountBalances
{
    [Trait("Category", "AccountBalance")]
    public class GetAccountBalanceQueryTest
    {
        private readonly IBankContext _bankContext;
        private readonly GetAccountBalanceQueryHandler _handler;
        private readonly IAccountBalanceCacheService _accountBalanceCacheService;

        public GetAccountBalanceQueryTest()
        {   
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AccountBalanceProfile())).CreateMapper();
            _bankContext = BankContextTestFactory.CreateSqlLiteContext();
            var mediator = new Mock<IMediator>().Object;                

            var cacheOptions = Options.Create(new MemoryDistributedCacheOptions());
            var cache = new MemoryDistributedCache(cacheOptions);

            _accountBalanceCacheService = new AccountBalanceCacheService(cache);

            _handler = new GetAccountBalanceQueryHandler(mapper, mediator, _bankContext, _accountBalanceCacheService);

            SetUp();
        }

        [Fact(DisplayName = "Searching valid existing account balance in context should return value")]        
        public async Task SearchingValidExistingAccountBalanceInContext_ShouldReturnValue()
        {
            var query = new GetAccountBalanceQuery(1);
            var response = await _handler.Handle(query, default);

            Assert.True(response.Value == 10);
        }

        [Fact(DisplayName = "Searching valid existing account balance in cache should return value")]
        public async Task SearchingValidExistingAccountBalanceInCache_ShouldReturnValue()
        {
            var query = new GetAccountBalanceQuery(2);
            var response = await _handler.Handle(query, default);

            Assert.True(response.Value == 25);
        }

        [Fact(DisplayName = "Searching non existing account should throws exception")]
        public async Task SearchingNonExistingAccount_ShouldThrowsException()
        {
            var query = new GetAccountBalanceQuery(999);
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>  _handler.Handle(query, default));
            Assert.Equal($"Account balance not found.", exception.Message);
        }

        [Fact(DisplayName = "If send default account id should throws exception")]
        public async Task SendingDefaultAccountId_ShouldThrowsException()
        {
            var command = new GetAccountBalanceQuery(0);
            var validator = new GetAccountBalanceQueryValidator();

            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(p => p.AccountId);
        }

        private void SetUp()
        {
            var accountBalance = new AccountBalance
            {
                AccountBalanceId = 1,
                AccountId = 1,
                Value = 10,
                LastTimeChanged = DateTime.UtcNow

            };

            _bankContext.AccountBalances.Add(accountBalance);
            _bankContext.SaveChangesAsync().GetAwaiter().GetResult();

            var model = new AccountBalanceCacheModel { AccountId = 2, Value = 25 };
            _accountBalanceCacheService.Set($"AccountId:2", model, default).GetAwaiter().GetResult();
        }
    }
}
