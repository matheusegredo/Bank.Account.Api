using Bank.Infrastructure.Cache.Interfaces;
using Bank.Infrastructure.Cache.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace Bank.Api.Infrastructure.Authentication
{
    public class CachedTokenAuthorize : ActionFilterAttribute
    {   
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var (successCache, cache) = TryGetCacheInstance(context);
            if (!successCache)
            {
                AuthorizationResult(context, HttpStatusCode.ServiceUnavailable, "Cache unavailable.");
                return;
            }

            var (successToken, receivedToken) = TryGetToken(context);
            if (!successToken)
            {
                AuthorizationResult(context, HttpStatusCode.Unauthorized);
                return;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            if (!tokenHandler.CanReadToken(receivedToken))
            {
                AuthorizationResult(context, HttpStatusCode.Unauthorized);
                return;
            }

            var jwtSecurityToken = tokenHandler.ReadToken(receivedToken) as JwtSecurityToken;
            var (userValid, statusCode) = ValidateUserToken(jwtSecurityToken, receivedToken, cache);
            if (!userValid)
            {
                AuthorizationResult(context, statusCode);
                return;
            }

            await next();

            return;
        }

        private void AuthorizationResult(ActionExecutingContext context, HttpStatusCode statusCode, string? content = null)
        {
            context.Result = new ContentResult() { StatusCode = (int)statusCode, Content = content };
        }

        private (bool success, string token) TryGetToken(ActionExecutingContext context)
        {
            var tokenHeader = context.HttpContext.Request.Headers[HeaderNames.Authorization].FirstOrDefault();

            if (tokenHeader is null)
                return (false, string.Empty);

             var token = tokenHeader.Replace("Bearer ", string.Empty);

            return string.IsNullOrEmpty(token) ? (false, string.Empty) : (true, token);
        }

        private (bool success, IAuthenticationJwtCacheService? cacheRepository) TryGetCacheInstance(ActionExecutingContext context)
        {
            if (context.HttpContext.RequestServices.GetService(typeof(IAuthenticationJwtCacheService)) is not IAuthenticationJwtCacheService cacheRepository)
                return (false, null);
            
            return (true, cacheRepository);
        }

        private (bool, HttpStatusCode) ValidateUserToken(JwtSecurityToken jwtSecurityToken, string receivedToken, IAuthenticationJwtCacheService cache)
        {
            var accountNumberClaim = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Actor);

            if (accountNumberClaim is null)
                return (false, HttpStatusCode.Unauthorized);

            var accountNumber = accountNumberClaim.Value;

            if (!cache.TryGet(accountNumber, out AuthenticationJwtCacheModel authentication))
                return (false, HttpStatusCode.Unauthorized);

            if (receivedToken != authentication.Token)
                return (false, HttpStatusCode.Unauthorized);           

            return (true, HttpStatusCode.OK);
        }
    }
}
