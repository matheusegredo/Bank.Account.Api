namespace Bank.Application.Commands.Accounts.Authentication
{
    public class AuthenticationAccountCommand : IRequest<AuthenticationAccountCommandResponse>
    {
        public string AccountNumber { get; set; }
    }
}
