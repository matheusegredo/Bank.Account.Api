global using Bank.Persistence.Interfaces;
using Bank.Application.Commands.AccountBalances.Notifications;
using Bank.Application.Helpers;
using Bank.CrossCutting.Exceptions;
using Bank.Data.Entities;
using Bank.Infrastructure.Authentication.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Commands.AccountMovimentations.Post
{
    public class PostAccountMovimentationCommandHandler : IRequestHandler<PostAccountMovimentationCommand, PostAccountMovimentationCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IBankContext _bankContext;
        private readonly IAuthenticationHelper _authenticationHelper;

        public PostAccountMovimentationCommandHandler(IMapper mapper, IMediator mediator, IBankContext bankContext, IAuthenticationHelper authenticationHelper)
        {
            _mapper = mapper;
            _mediator = mediator;
            _bankContext = bankContext;
            _authenticationHelper = authenticationHelper;
        }

        public async Task<PostAccountMovimentationCommandResponse> Handle(PostAccountMovimentationCommand command, CancellationToken cancellationToken)
        {
            var account = await _bankContext.Accounts
                .Where(account => account.AccountId == command.AccountId)
                .FirstOrDefaultReadingUncomittedAsync(cancellationToken);

            if (account is null)
                throw new NotFoundException("Account not found.");

            _authenticationHelper.ValidateTokenByAccount(account, cancellationToken);

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

            await _mediator.Publish(new PutAccountBalanceNotification(account.AccountId, accountMovimentation.Value), cancellationToken);            

            return new PostAccountMovimentationCommandResponse("Account movimentation inserted succefully!");
        }
    }
}
