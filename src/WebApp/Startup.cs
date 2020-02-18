using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Codidact.Authentication.Application;
using Codidact.Authentication.Infrastructure;

using Microsoft.AspNetCore.Identity;
using Codidact.Authentication.Domain.Entities;

namespace Codidact.Authentication.WebApp
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.AddInfrastructure(_configuration);
            services.AddApplication(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.EnvironmentName != "Testing")
            {
                throw new NotImplementedException(env.EnvironmentName);
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            // Todo. I removed and re-added this a couple of times, I should finally make up my mind.
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var users = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                users.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@codidact",
                    Email = "admin@codidact"
                }, "password");
            }
        }
    }
}
