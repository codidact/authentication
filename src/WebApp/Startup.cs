using System;
using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Codidact.Authentication.Application;
using Codidact.Authentication.Infrastructure;
using Codidact.Authentication.Infrastructure.Persistance;
using Codidact.Authentication.Domain.Entities;

namespace Codidact.Authentication.WebApp
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;

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

            SeedDatabase(app);
        }

        private static void SeedDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var users = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

                // Todo. Use database migrations in the future.
                db.Database.EnsureCreated();

                // Todo. Remove this when we have a registration page.
                if (!users.Users.Any())
                {
                    users.CreateAsync(new ApplicationUser
                    {
                        UserName = "admin@codidact",
                        Email = "admin@codidact"
                    }, "password");
                }

                db.SaveChanges();
            }
        }
    }
}
