using Bank.Infrastructure.Authentication.Helpers;
using Bank.Infrastructure.Authentication.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Infrastructure.Authentication
{
    public static class DependencyInjection
    {
        public static void AddAuthenticationDependencies(this IServiceCollection services) 
        {
            services.AddScoped<IAuthenticationHelper, AuthenticationHelper>();
        }
    }
}
