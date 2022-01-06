using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bank.Infrastructure.Authentication
{
    public sealed class JwtTokenGenerator
    {
        public string GenerateToken(string accountNumber, DateTime expiresOn)
        {
            var symmetricKey = new SymmetricSecurityKey(Convert.FromBase64String("oCkI5llg61tfSwJjNwzrgw4nV77dK3ze6sNW"));
            var now = DateTime.Now;

            var tokenSpecifications = new JwtSecurityToken(
                    "Warren",
                    "Bank.Api",
                    new List<Claim> { new Claim(ClaimTypes.Actor, accountNumber) },
                    now,
                    expiresOn,
                    new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(tokenSpecifications);
        }
    }
}
