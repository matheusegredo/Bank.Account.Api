global using MediatR;

namespace Bank.Application.Commands.Clients.Post
{
    public class PostClientCommand : IRequest<PostClientCommandResponse>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
