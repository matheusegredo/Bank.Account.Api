namespace Bank.Infrastructure.Cache.Models.Authentication
{
    public class AuthenticationJwtCacheModel
    {
        public AuthenticationJwtCacheModel(string token, string tokenIdentifier)
        {
            Token = token;
            TokenIdentifier = tokenIdentifier;
        }

        public string Token { get; set; }

        public string TokenIdentifier { get; set; }
    }
}
