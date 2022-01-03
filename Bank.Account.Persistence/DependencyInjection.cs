using Bank.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Persistence
{
    public static class DependencyInjection
    {
        public static void AddPersistenceDependencies(this IServiceCollection services, IConfiguration configuration) 
        {
            var conn = configuration.GetConnectionString("bankContext");
            services.AddDbContextPool<IBankContext, BankContext>(
                c => c.UseMySQL(conn));                            
        }
    }
}
