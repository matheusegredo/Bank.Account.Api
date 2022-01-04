namespace Bank.Application.Commands.Accounts
{
    public class PostAccontCommandHandler : IRequestHandler<PostAccontCommand, PostAccontCommandResponse>
    {
        public Task<PostAccontCommandResponse> Handle(PostAccontCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
