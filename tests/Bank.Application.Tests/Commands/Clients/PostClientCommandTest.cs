using AutoMapper;
using Bank.Application.Commands.Clients.Post;
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
    [Trait("Category", "Client")]
    public class PostClientCommandTest
    {
        private readonly IBankContext _bankContext;
        private readonly PostClientCommandHandler _handler;

        public PostClientCommandTest()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile(new ClientProfile())).CreateMapper();
            _bankContext = BankContextTestFactory.CreateSqlLiteContext();

            _handler = new PostClientCommandHandler(mapper, _bankContext);

            SetUp();
        }

        [Fact(DisplayName = "Sending valid contract should create client")]
        public async Task SendingValidContract_ShouldCreateClient()
        {
            var command = new PostClientCommand { Email = "valid@email.com", FirstName = "first", LastName = "last" };

            await _handler.Handle(command, default);

            Assert.Contains(_bankContext.Clients, client => client.Email == command.Email);
        }

        [Fact(DisplayName = "If email exist should throws exception")]
        public async Task UsingExistingEmail_ShouldThrowsException()
        {
            var command = new PostClientCommand { Email = "email@test.com" };

            await Assert.ThrowsAsync<InvalidRequestException>(() => _handler.Handle(command, default));
        }

        [Fact(DisplayName = "If send empty or null FirstName should throws exception")]
        public async Task SendingEmptyOrNullFirstName_ShouldThrowsException()
        {
            var command = new PostClientCommand { FirstName = string.Empty };
            var validator = new PostClientCommandValidator();

            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(p => p.FirstName);
        }

        [Fact(DisplayName = "If send empty or null LastName should throws exception")]
        public async Task SendingEmptyOrNullLastName_ShouldThrowsException()
        {
            var command = new PostClientCommand { LastName = string.Empty };
            var validator = new PostClientCommandValidator();
            
            var result =  await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(p => p.LastName);
        }

        [Fact(DisplayName = "If send empty or null Email should throws exception")]
        public async Task SendingEmptyOrNullEmail_ShouldThrowsException()
        {
            var command = new PostClientCommand { Email = string.Empty };
            var validator = new PostClientCommandValidator();

            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(p => p.Email);
        }

        [Fact(DisplayName = "If send invalid Email should throws exception")]
        public async Task SendingInvalidEmail_ShouldThrowsException()
        {
            var command = new PostClientCommand { Email = "invalid.email.com" };
            var validator = new PostClientCommandValidator();

            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(p => p.Email);
        }


        private void SetUp() 
        {
            var client = new Client
            {
                ClientId = 1,
                FirstName = "first",
                LastName = "last",
                Email = "email@test.com"
            };

            _bankContext.Clients.Add(client);
            _bankContext.SaveChangesAsync().GetAwaiter().GetResult();
        } 
    }
}
