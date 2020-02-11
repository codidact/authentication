using Microsoft.Extensions.DependencyInjection;

namespace Codidact.Authentication.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}
