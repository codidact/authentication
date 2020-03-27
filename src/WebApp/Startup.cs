using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Codidact.Authentication.Application;
using Codidact.Authentication.Infrastructure;
using Codidact.Authentication.Infrastructure.Persistance;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Codidact.Authentication.WebApp
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment, ILogger<Startup> logger)
        {
            _configuration = configuration;
            _environment = environment;
            _logger = logger;

            if (!environment.IsDevelopment() && !environment.IsProduction())
            {
                throw new NotImplementedException($"The environment '{environment.EnvironmentName}' has not been configured yet.");
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddRouting(options =>
                {
                    options.LowercaseUrls = true;
                });

            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.AddInfrastructure(_configuration, _environment);
            services.AddApplication(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsProduction())
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            if (env.EnvironmentName != "Test")
            {
                ApplyDatabaseMigrations(app, _logger);
            }
        }
        /// <summary>
        // Applies database migrations; won't cause any changes if the database is up-to-date.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="logger"></param>
        private void ApplyDatabaseMigrations(IApplicationBuilder app, ILogger<Startup> logger)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    try
                    {
                        context.Database.Migrate();
                    }
                    catch (System.Exception ex)
                    {
                        logger.LogError("Unable to apply database migrations. Check the connection string in your " +
                            "appsettings file.");
                        throw ex;
                    }
                }
            }
        }
    }
}
