using Bank.Application.Helpers;
using Bank.CrossCutting.Exceptions;
using Bank.Data.Entities;

namespace Bank.Application.Commands.Clients.Post
{
    public class PostClientCommandHandler : IRequestHandler<PostClientCommand, PostClientCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBankContext _bankContext;

        public PostClientCommandHandler(IMapper mapper, IBankContext bankContext)
        {
            _mapper = mapper;
            _bankContext = bankContext;
        }

        public async Task<PostClientCommandResponse> Handle(PostClientCommand command, CancellationToken cancellationToken)
        {
            var existingClientEmail = await _bankContext.Clients
                .Where(client => client.Email == command.Email)
                .Select(client => new Client { ClientId = client.ClientId })
                .FirstOrDefaultReadingUncomittedAsync(cancellationToken);

            if (existingClientEmail is not null)
                throw new InvalidRequestException("Client email already exist.");

            var client = _mapper.Map<Client>(command);

            _bankContext.Clients.Add(client);
            await _bankContext.SaveChangesAsync(cancellationToken);

            return new PostClientCommandResponse("Client inserted successfully!");
        }
    }
}
