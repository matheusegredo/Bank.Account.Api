using Bank.Application.Commands.Clients.Post;
using Bank.Application.Pipelines;
using Bank.Application.Profiles;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Bank.Application
{
    public static class DependencyInjection
    {
        public static void AddApplicationDependecies(this IServiceCollection services) 
        {
            services.AddAutoMapper(typeof(ClientProfile).Assembly);
            services.AddMediatR(typeof(PostClientCommand).Assembly);
            InjectValidators(services);            

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorRequestBehavior<,>));
        }

        private static void InjectValidators(IServiceCollection services) 
        {
            services.AddValidatorsFromAssembly(typeof(PostClientCommandValidator).Assembly);
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");
        }
    }
}
