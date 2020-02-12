using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Codidact.Authentication.Application.Extensions;
using Codidact.Authentication.Infrastructure.Extensions;
using Codidact.Authentication.Infrastructure.Identity;

namespace Codidact.Authentication.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddInfrastructure();
            services.AddApplication();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CreateAdminAccount(app).Wait();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Todo. Implement this for production.
                throw new NotImplementedException();
            }

            app.UseAuthentication();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }

        private async Task CreateAdminAccount(IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();

            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            var logger = scope.ServiceProvider.GetService<ILogger<Startup>>();

            if (await userManager.FindByNameAsync("admin") == null)
            {
                var result = await userManager.CreateAsync(new ApplicationUser
                {
                    Email = "admin@codidact",
                    UserName = "admin@codidact",
                }, "password");

                if (!result.Succeeded)
                {
                    throw new Exception("Could not create an administrator account.");
                }

                logger.LogInformation("No 'admin@codidact' user found, created with password 'password'.");
            }
        }
    }
}
