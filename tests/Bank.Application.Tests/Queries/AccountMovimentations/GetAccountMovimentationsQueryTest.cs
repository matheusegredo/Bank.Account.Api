using Bank.Application.Queries.AccountBalances;
using Bank.Application.Queries.AccountMovimentations.Get;
using Bank.Application.Tests.Moq;
using Bank.CrossCutting.Exceptions;
using Bank.Data;
using Bank.Data.Entities;
using Bank.Infrastructure.Authentication.Interfaces;
using Bank.Persistence.Interfaces;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Bank.Application.Tests.Commands.AccountBalances
{
    [Trait("Category", "AccountMovimentation")]
    public class GetAccountMovimentationsQueryTest
    {
        private readonly IBankContext _bankContext;
        private readonly GetAccountMovimentationsQueryHandler _handler;

        public GetAccountMovimentationsQueryTest()
        {   
            _bankContext = BankContextTestFactory.CreateSqlLiteContext();
            var mediator = new Mock<IMediator>();
            mediator.Setup(p =>
                p.Send(It.IsAny<GetAccountBalanceQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAccountBalanceQueryResponse { AccountId = 1, Value = 20 });

            var authentication = new Mock<IAuthenticationHelper>().Object;

            _handler = new GetAccountMovimentationsQueryHandler(mediator.Object, _bankContext, authentication);

            SetUp();
        }

        [Fact(DisplayName = "Searching all movimentations with existing account should return 2 elements")]        
        public async Task SearchingAllMovimentations_WithExistingAccount_ShouldReturnValue()
        {
            var query = new GetAccountMovimentationsQuery { AccountId = 1};
            var response = await _handler.Handle(query, default);

            Assert.True(response.Movimentations.Count == 2);
        }

        [Fact(DisplayName = "Searching daily movimentations with existing account should return 1 elements")]
        public async Task SearchingDailyMovimentations_WithExistingAccount_ShouldReturnValue()
        {
            var query = new GetAccountMovimentationsQuery { AccountId = 1, InitialDate = DateTime.UtcNow, FinalDate = DateTime.UtcNow };
            var response = await _handler.Handle(query, default);

            Assert.True(response.Movimentations.Count == 1);
            Assert.Contains(response.Movimentations, movimentations => movimentations.Date == DateTime.UtcNow.ToShortDateString());
        }

        [Fact(DisplayName = "Searching non existing account should throws exception")]
        public async Task SearchingNonExistingAccount_ShouldThrowsException()
        {
            var query = new GetAccountMovimentationsQuery { AccountId = 999 };
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>  _handler.Handle(query, default));
            Assert.Equal($"Account not found.", exception.Message);
        }

        [Fact(DisplayName = "If send default account id should throws exception")]
        public async Task SendingDefaultAccountId_ShouldThrowsException()
        {
            var command = new GetAccountMovimentationsQuery { AccountId = 0};
            var validator = new GetAccountMovimentationsQueryValidator();

            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(p => p.AccountId);
        }

        [Fact(DisplayName = "If send null initial date and correct final date should throws exception")]
        public async Task SendingInitialDateAndNullFinalDate_ShouldThrowsException()
        {
            var command = new GetAccountMovimentationsQuery { InitialDate = DateTime.UtcNow, FinalDate = null };
            var validator = new GetAccountMovimentationsQueryValidator();

            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(p => p.FinalDate);
        }

        [Fact(DisplayName = "If send correct initial date and null final date should throws exception")]
        public async Task SendingNullInitialDateAndFinalDate_ShouldThrowsException()
        {
            var command = new GetAccountMovimentationsQuery { FinalDate = DateTime.UtcNow, InitialDate = null };
            var validator = new GetAccountMovimentationsQueryValidator();

            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(p => p.InitialDate);
        }

        [Fact(DisplayName = "If send initial date greater than final date should throws exception")]
        public async Task SendingInitialDateGreaterThanFinalDate_ShouldThrowsException()
        {
            var command = new GetAccountMovimentationsQuery { FinalDate = DateTime.UtcNow, InitialDate = DateTime.UtcNow.AddDays(1) };
            var validator = new GetAccountMovimentationsQueryValidator();

            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(p => p.FinalDate);
        }

        private void SetUp()
        {
            var accountBalance = new AccountBalance
            {
                AccountBalanceId = 1,                
                Value = 10,
                LastTimeChanged = DateTime.UtcNow               
            };

            var accountMovimentations = new List<AccountMovimentation>
            {
                new AccountMovimentation { Value = 40, Type = MovimentationType.Deposit },
                new AccountMovimentation { Value = 20, Type = MovimentationType.Rescue },
            };

            var account = new Account
            {
                AccountId = 1,
                AccountBalance = accountBalance,
                Movimentations = accountMovimentations,
                AccountNumber = string.Empty
            };

            _bankContext.Accounts.Add(account);
            _bankContext.SaveChangesAsync().GetAwaiter().GetResult();

            var accountMovimentation = _bankContext.AccountMovimentations.FirstOrDefault();
            accountMovimentation.CreatedOn = DateTime.UtcNow.AddDays(10);

            _bankContext.SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
