using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Codidact.Authentication.Domain.Entities;
using Codidact.Authentication.Application.Options;
using Codidact.Authentication.Application.Common.Interfaces;
using Codidact.Authentication.Infrastructure.Persistance;
using Codidact.Authentication.Infrastructure.Services;

namespace Codidact.Authentication.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.AddScoped<ICoreApiService, CoreApiService>();

            services
                .AddDbContext<ApplicationDbContext>((provider, options) =>
                {
                    options.UseNpgsql(
                        configuration.GetConnectionString("DefaultConnection"),
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
                .AddSignInManager()
                .AddDefaultTokenProviders();

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

            services.Configure<MailOptions>(configuration.GetSection("MailServer"));

            if (environment.IsDevelopment())
            {
                services.AddScoped<IMailService, DevelopmentMailService>();
            }
            else
            {
                services.AddScoped<IMailService, MailService>();
            }

            return services;
        }
    }
}
