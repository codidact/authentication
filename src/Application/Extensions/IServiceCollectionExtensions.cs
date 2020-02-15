using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using Codidact.Authentication.Application.Services;

namespace Codidact.Authentication.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<InitializationService>();

            return services;
        }
    }
}
