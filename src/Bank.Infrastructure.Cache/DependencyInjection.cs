using Bank.Infrastructure.Cache.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Infrastructure.Cache
{
    public static class DependencyInjection
    {
        public static void AddCacheDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedRedisCache(opt =>
            {
                opt.Configuration = configuration.GetConnectionString("redis");
                opt.InstanceName = "Bank.Api";
            });

            services.AddScoped<IAccountBalanceCacheService, AccountBalanceCacheService>();
        }
    }
}
