using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using Codidact.Authentication.Infrastructure.Persistance;
using Codidact.Authentication.Infrastructure.Identity;

namespace Codidact.Authentication.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("authentication"));

            services.AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddAspNetIdentity<ApplicationUser>();

            return services;
        }
    }
}
