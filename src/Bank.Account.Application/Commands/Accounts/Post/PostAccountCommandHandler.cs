using Bank.Application.Helpers;
using Bank.CrossCutting.Exceptions;
using Bank.Data.Entities;
using Bank.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Commands.Accounts.Post
{
    public class PostAccountCommandHandler : IRequestHandler<PostAccountCommand, PostAccountCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBankContext _bankContext;

        public PostAccountCommandHandler(IMapper mapper, IBankContext bankContext)
        {
            _mapper = mapper;
            _bankContext = bankContext;
        }

        public async Task<PostAccountCommandResponse> Handle(PostAccountCommand command, CancellationToken cancellationToken)
        {
            var clientId = await _bankContext.GetValueByKey<Client>(command.ClientId);

            if (clientId is default(int))
                throw new NotFoundException("Client not found.");

            var existingAccountNumber = await _bankContext.Accounts
                .Where(account => account.AccountNumber == command.AccountNumber)
                .Select(account => new Account { AccountId = account.AccountId })
                .FirstOrDefaultAsync(cancellationToken);

            if (existingAccountNumber is not null)
                throw new InvalidRequestException($"The account number {command.AccountNumber} already exist.");

            var clientWithAccount = await _bankContext.Accounts
                .Where(account => account.ClientId == command.ClientId)
                .Select(account => new Account { AccountId = account.AccountId, Client = new Client { FirstName = account.Client.FirstName, LastName = account.Client.LastName } })
                .FirstOrDefaultAsync(cancellationToken);

            if (clientWithAccount is not null)
                throw new InvalidRequestException($"{clientWithAccount.Client.GetFullName} already have a account!");

            var account = _mapper.Map<Account>(command);

            _bankContext.Accounts.Add(account);
            await _bankContext.SaveChangesAsync();

            return new PostAccountCommandResponse("Account created successfully!");
        }
    }
}
