global using Bank.Persistence.Interfaces;
using Bank.Application.Helpers;
using Bank.CrossCutting.Exceptions;
using Bank.Data.Entities;

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
        }
    }
}
