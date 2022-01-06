using AutoMapper;
using Bank.Application.Commands.AccountMovimentations.Post;
using Bank.Application.Profiles;
using Bank.Application.Tests.Factory;
using Bank.Application.Tests.Moq;
using Bank.CrossCutting.Exceptions;
using Bank.Data;
using Bank.Data.Entities;
using Bank.Infrastructure.Authentication.Interfaces;
using Bank.Persistence.Interfaces;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Bank.Application.Tests.Commands.AccountMovimentations
{
    [Trait("Category", "AccountMovimentation")]
    public class PostAccountMovimentationCommandTest
    {
        private readonly IBankContext _bankContext;
        private readonly PostAccountMovimentationCommandHandler _handler;

        public PostAccountMovimentationCommandTest()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AccountMovimentationProfile())).CreateMapper();
            _bankContext = BankContextTestFactory.CreateSqlLiteContext();
            var mediator = new Mock<IMediator>().Object;
            var authentication = new Mock<IAuthenticationHelper>().Object;

            _handler = new PostAccountMovimentationCommandHandler(mapper, mediator, _bankContext, authentication);

            SetUp();
        }

        [Theory(DisplayName = "Sending valid contract should create account movimentation")]
        [InlineData(MovimentationType.Yield)]
        [InlineData(MovimentationType.Rescue)]
        [InlineData(MovimentationType.Payment)]
        [InlineData(MovimentationType.Deposit)]
        public async Task SendingValidContract_ShouldCreateClient(MovimentationType type)
        {
            var command = new PostAccountMovimentationCommand { AccountId = 1, Value = 100, Type = type };

            if (!command.ValueShouldBePositive)
                command.Value *= -1;

            await _handler.Handle(command, default);

            Assert.Contains(_bankContext.AccountMovimentations, accountMovimentation => 
                accountMovimentation.AccountId == command.AccountId && accountMovimentation.Value == command.Value && accountMovimentation.Type == command.Type);
        }

        [Theory(DisplayName = "Sending valid contract without balance should throws exception")]
        [InlineData(MovimentationType.Payment)]
        [InlineData(MovimentationType.Rescue)]        
        public async Task SendingValidContractWithoutBalance_ShouldThrowsException(MovimentationType type)
        {
            var command = new PostAccountMovimentationCommand { AccountId = 1, Value = -101, Type = type };

            var exception = await Assert.ThrowsAsync<InvalidRequestException>(() => _handler.Handle(command, default));
            Assert.Equal("Insufficient balance to carry out the transaction", exception.Message);
        }

        [Fact(DisplayName = "If account not exist should throws exception")]
        public async Task UsingNonExistingAccountId_ShouldThrowsException()
        {
            var command = new PostAccountMovimentationCommand { AccountId = 999 };

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
            Assert.Equal($"Account not found.", exception.Message);
        }

        [Fact(DisplayName = "If send default account id should throws exception")]
        public async Task SendingDefaultAccountId_ShouldThrowsException()
        {
            var command = new PostAccountMovimentationCommand { AccountId = 0 };
            var validator = new PostAccountMovimentationCommandValidator();

            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(p => p.AccountId);
        }

        [Fact(DisplayName = "If send MovimentationType out of range should throws exception")]
        public async Task SendingMovimentationTypeOutOfRange_ShouldThrowsException()
        {
            var command = new PostAccountMovimentationCommand { Type = (MovimentationType) OutOfRangeEnumFactory.Generate<MovimentationType>() };
            var validator = new PostAccountMovimentationCommandValidator();

            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(p => p.Type);
        }

        [Fact(DisplayName = "If send MovimentationType.Deposit and value negative should throws exception")]
        public async Task SendingDepositAndValueNegative_ShouldThrowsException()
        {
            var command = new PostAccountMovimentationCommand { Type = MovimentationType.Deposit, Value = -1 };
            var validator = new PostAccountMovimentationCommandValidator();

            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(p => p.Value);
        }

        [Fact(DisplayName = "If send MovimentationType.Rescue and value positive should throws exception")]
        public async Task SendingRescueAndValuePositive_ShouldThrowsException()
        {
            var command = new PostAccountMovimentationCommand { Type = MovimentationType.Rescue, Value = 1 };
            var validator = new PostAccountMovimentationCommandValidator();

            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(p => p.Value);
        }

        private void SetUp()
        {
            var account = new Account
            {
                AccountId = 1,
                ClientId = 1,
                AccountNumber = string.Empty,
                AccountBalance = new AccountBalance { Value = 100 }
            };

            _bankContext.Accounts.Add(account);
            _bankContext.SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
