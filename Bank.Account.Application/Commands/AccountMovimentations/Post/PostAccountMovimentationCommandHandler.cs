namespace Bank.Application.Commands.AccountMovimentations.Post
{
    internal class PostAccountMovimentationCommandHandler : IRequestHandler<PostAccountMovimentationCommand, PostAccountMovimentationCommandResponse>
    {
        public Task<PostAccountMovimentationCommandResponse> Handle(PostAccountMovimentationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
