namespace Bank.Application.Commands.Accounts.Authentication
{
    public class AuthenticationAccountCommandResponse
    {
        public AuthenticationAccountCommandResponse(string token, DateTime expiresOn)
        {
            Token = token;
            ExpiresOn = expiresOn;
        }

        public string Token { get; set; }

        public DateTime ExpiresOn { get; set; }
    }
}
