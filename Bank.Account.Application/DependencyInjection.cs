using Bank.Application.Commands.Clients.Post;
using Bank.Application.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Application
{
    public static class DependencyInjection
    {
        public static void AddApplicationDependecies(this IServiceCollection services) 
        {
            services.AddAutoMapper(typeof(ClientProfile).Assembly);
            services.AddMediatR(typeof(PostClientCommand).Assembly);
        }
    }
}
