namespace Bank.Application.Commands.Accounts.Post
{
    public class PostAccountCommand : IRequest<PostAccountCommandResponse>
    {
        public int ClientId { get; set; }

        public string AccountNumber { get; set; }
    }
}
