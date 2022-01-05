using AutoMapper;
using Bank.Application.Commands.AccountBalances.Notifications;
using Bank.Application.Profiles;
using Bank.Application.Tests.Moq;
using Bank.Data.Entities;
using Bank.Persistence.Interfaces;
using MediatR;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Bank.Application.Tests.Commands.AccountBalances
{
    [Trait("Category", "AccountBalance")]
    public class PutAccountBalanceNotificationTest
    {
        private readonly IBankContext _bankContext;
        private readonly PutAccountBalanceNotificationHandler _handler;

        private readonly DateTime _lastTimeChanged = DateTime.UtcNow;

        public PutAccountBalanceNotificationTest()
        {   
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AccountBalanceProfile())).CreateMapper();
            _bankContext = BankContextTestFactory.CreateSqlLiteContext();
            var mediator = new Mock<IMediator>().Object;

            _handler = new PutAccountBalanceNotificationHandler(mapper, mediator, _bankContext);

            SetUp();
        }

        [Theory(DisplayName = "Sending valid existing account balance should update value")]
        [InlineData(10)]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(25.5)]
        public async Task SendingValidExistingAccountBalance_ShouldUpdateValue(decimal value)
        {
            var notification = new PutAccountBalanceNotification(1, value);
            await _handler.Handle(notification, default);

            Assert.Contains(_bankContext.AccountBalances, accountBalance => 
                accountBalance.AccountId == 1 &&
                accountBalance.Value == 10 + value && 
                accountBalance.LastTimeChanged != _lastTimeChanged);
        }

        [Theory(DisplayName = "Sending valid nonexisting account balance should update value")]
        [InlineData(10)]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(25.5)]
        public async Task SendingValidNonExistingAccountBalance_ShouldUpdateValue(decimal value)
        {
            var notification = new PutAccountBalanceNotification(2, value);
            await _handler.Handle(notification, default);

            Assert.Contains(_bankContext.AccountBalances, accountBalance => 
                accountBalance.AccountId == 2 && 
                accountBalance.Value == value && 
                accountBalance.LastTimeChanged != _lastTimeChanged);
        }

        private void SetUp()
        {
            var accountBalance = new AccountBalance
            {
                AccountBalanceId = 1,
                AccountId = 1,
                Value = 10,
                LastTimeChanged = _lastTimeChanged

            };

            _bankContext.AccountBalances.Add(accountBalance);
            _bankContext.SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
