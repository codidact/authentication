using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Codidact.Authentication.Application.Extensions;
using Codidact.Authentication.Infrastructure.Extensions;
using Codidact.Authentication.Infrastructure.Persistance;
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
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.AddInfrastructure();
            services.AddApplication();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CreateDummyData(app);

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

        // Todo. Remove this as soon as possible.
        void CreateDummyData(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            var logger = scope.ServiceProvider.GetService<ILogger<Startup>>();

            if (db.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
            {
                var result = userManager.CreateAsync(new ApplicationUser
                {
                    Email = "admin@codidact",
                    UserName = "admin@codidact"
                }, "password").Result;

                if (!result.Succeeded)
                {
                    throw new Exception("Could not create admin user.");
                }

                logger.LogWarning("Created 'admin@codidact' user with password 'password'.");
            }
            else
            {
                throw new Exception("Please delete this code.");
            }
        }
    }
}
