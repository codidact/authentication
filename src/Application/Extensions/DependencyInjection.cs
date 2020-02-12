using Microsoft.Extensions.DependencyInjection;

using Codidact.Authentication.Application.Services;

namespace Codidact.Authentication.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<InitializationService>();

            return services;
        }
    }
}
