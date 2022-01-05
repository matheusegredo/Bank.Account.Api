global using Bank.Persistence.Interfaces;
using Bank.Application.Commands.AccountBalances.Notifications;
using Bank.Application.Helpers;
using Bank.CrossCutting.Exceptions;
using Bank.Data;
using Bank.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Commands.AccountMovimentations.Post
{
    public class PostAccountMovimentationCommandHandler : IRequestHandler<PostAccountMovimentationCommand, PostAccountMovimentationCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IBankContext _bankContext;

        public PostAccountMovimentationCommandHandler(IMapper mapper, IMediator mediator, IBankContext bankContext)
        {
            _mapper = mapper;
            _mediator = mediator;
            _bankContext = bankContext;
        }

        public async Task<PostAccountMovimentationCommandResponse> Handle(PostAccountMovimentationCommand command, CancellationToken cancellationToken)
        {
            var accountId = await _bankContext.GetValueByKey<Account>(command.AccountId);

            if (accountId is default(int))
                throw new NotFoundException("Account not found.");

            var balance = await _bankContext.AccountBalances
                .Where(accountBalance => accountBalance.AccountId == command.AccountId)
                .Select(accountBalance => new AccountBalance { Value = accountBalance.Value })
                .FirstOrDefaultAsync(cancellationToken);

            if (balance is null)
                balance = new AccountBalance { Value = 0 };

            if (balance.Value + command.Value < 0)
                throw new InvalidRequestException("Insufficient balance to carry out the transaction");
            
            var accountMovimentation = _mapper.Map<AccountMovimentation>(command);

            _bankContext.AccountMovimentations.Add(accountMovimentation);
            await _bankContext.SaveChangesAsync();

            await _mediator.Publish(new PutAccountBalanceNotification(accountId, accountMovimentation.Value), cancellationToken);            

            return new PostAccountMovimentationCommandResponse("Account movimentation inserted succefully!");
        }
    }
}
