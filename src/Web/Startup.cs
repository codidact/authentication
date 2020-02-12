using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Codidact.Authentication.Application.Extensions;
using Codidact.Authentication.Infrastructure.Extensions;
using Codidact.Authentication.Infrastructure.Persistance;
using Codidact.Authentication.Application.Services;

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

            CreateDummyData(app).Wait();
        }

        // Todo. Remove this as soon as possible.
        private async Task CreateDummyData(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetService<ILogger<Startup>>();
            var init = scope.ServiceProvider.GetService<InitializationService>();

            if (db.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
            {
                var result = await init.CreateAdministratorAsync("admin@codidact", "password");

                if (result.Succeeded)
                {
                    logger.LogWarning("Created 'admin@codidact' user with password 'password'.");
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new Exception("Please delete this code.");
            }
        }
    }
}
