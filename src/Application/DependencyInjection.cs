using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Codidact.Authentication.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
