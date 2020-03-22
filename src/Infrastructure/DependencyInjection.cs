using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;

using Codidact.Authentication.Infrastructure.Persistance;
using Codidact.Authentication.Infrastructure.Common.Interfaces;
using Codidact.Authentication.Infrastructure.Services;
using Codidact.Authentication.Domain.Entities;
using Codidact.Authentication.Application.Common.Interfaces;

namespace Codidact.Authentication.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                services.AddScoped<ISecretsService, DevelopmentSecretsService>();
                services.AddScoped<ICoreApiService, DevelopmentCoreApiService>();
            }

            services
                .AddDbContext<ApplicationDbContext>(async (provider, options) =>
                {
                    var secrets = provider.GetService<ISecretsService>();
                    var connectionString = secrets.Get("ConnectionStrings:Authentication").GetAwaiter().GetResult();
                    options.UseNpgsql(connectionString,
                       b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
                });

            services
                .AddIdentityCore<ApplicationUser>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequiredUniqueChars = 1;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager();

            var identityServerBuilder = services.AddIdentityServer()
                .AddInMemoryClients(configuration.GetSection("IdentityServer:Clients"))
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddAspNetIdentity<ApplicationUser>();

            if (environment.IsDevelopment() || environment.EnvironmentName == "Testing")
            {
                identityServerBuilder.AddDeveloperSigningCredential();
            }

            services.AddAuthentication()
                .AddIdentityCookies();

            return services;
        }
    }
}
