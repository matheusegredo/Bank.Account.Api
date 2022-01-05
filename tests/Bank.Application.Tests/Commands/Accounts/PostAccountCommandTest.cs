using AutoMapper;
using Bank.Application.Commands.Accounts.Post;
using Bank.Application.Profiles;
using Bank.Application.Tests.Moq;
using Bank.CrossCutting.Exceptions;
using Bank.Data.Entities;
using Bank.Persistence.Interfaces;
using FluentValidation.TestHelper;
using System.Threading.Tasks;
using Xunit;

namespace Bank.Application.Tests.Commands.Clients
{
    [Trait("Category", "Account")]
    public class PostAccountCommandTest
    {
        private readonly IBankContext _bankContext;
        private readonly PostAccountCommandHandler _handler;

        public PostAccountCommandTest()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new AccountProfile())).CreateMapper();
            _bankContext = BankContextTestFactory.CreateSqlLiteContext();

            _handler = new PostAccountCommandHandler(mapper, _bankContext);

            SetUp();
        }

        [Fact(DisplayName = "Sending valid contract should create account")]
        public async Task SendingValidContract_ShouldCreateAccount()
        {
            var command = new PostAccountCommand { ClientId = 2, AccountNumber = "00001" };

            await _handler.Handle(command, default);

            Assert.Contains(_bankContext.Accounts, account => account.AccountNumber == command.AccountNumber);
        }

        [Fact(DisplayName = "If client not exist should throws exception")]
        public async Task UsingNonExistingClientId_ShouldThrowsException()
        {
            var command = new PostAccountCommand { ClientId = 999 };

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
            Assert.Equal($"Client not found.", exception.Message);
        }

        [Fact(DisplayName = "If Account Number already exist should throws exception")]
        public async Task UsingExistingAccountNumber_ShouldThrowsException()
        {
            var command = new PostAccountCommand { ClientId = 1, AccountNumber = "00000" };

            var exception = await Assert.ThrowsAsync<InvalidRequestException>(() => _handler.Handle(command, default));
            Assert.Equal($"The account number {command.AccountNumber} already exist.", exception.Message);
        }

        [Fact(DisplayName = "If Client already have Account should throws exception")]
        public async Task UsingClientWithAccount_ShouldThrowsException()
        {
            var command = new PostAccountCommand { ClientId = 1 };

            var exception = await Assert.ThrowsAsync<InvalidRequestException>(() => _handler.Handle(command, default));
            Assert.Equal("Name Lastname already have a account!", exception.Message);
        }

        [Fact(DisplayName = "If send default ClientId should throws exception")]
        public async Task SendingDefaultClientId_ShouldThrowsException()
        {
            var command = new PostAccountCommand { ClientId = 0 };
            var validator = new PostAccountCommandValidator();

            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(p => p.ClientId);
        }

        [Fact(DisplayName = "If send empty or null AccountNumber should throws exception")]
        public async Task SendingEmptyOrNullAccountNumber_ShouldThrowsException()
        {
            var command = new PostAccountCommand { AccountNumber = string.Empty };
            var validator = new PostAccountCommandValidator();
            
            var result =  await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(p => p.AccountNumber);
        }

        private void SetUp() 
        {
            var clientWithAccount = new Client
            {
                ClientId = 1,
                FirstName = "Name",
                LastName = "Lastname",
                Email = "email@test.com",
                Account = new Account 
                {
                    AccountNumber = "00000"
                }
            };

            var clientWithoutAccount = new Client
            {
                ClientId = 2,
                FirstName = "first",
                LastName = "last",
                Email = "email@test.com"
            };

            _bankContext.Clients.Add(clientWithAccount);
            _bankContext.Clients.Add(clientWithoutAccount);
            _bankContext.SaveChangesAsync().GetAwaiter().GetResult();
        } 
    }
}
