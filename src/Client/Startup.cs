using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Codidact.Authentication.Client
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IWebHostEnvironment environment)
        {
            this._environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
            }

            // Todo. What does this do?
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services
                .AddAuthentication(options =>
                {
                    // Todo. Verify that this configuration works as intended.
                    options.DefaultScheme = "cookie";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("cookie")
                .AddOpenIdConnect("oidc", options =>
                {
                    if (_environment.IsDevelopment())
                    {
                        // Do not verify server certificates. This is useful for development because you typically do not
                        // use a trusted certificate for localhost.
                        var httpHandler = new HttpClientHandler();
                        httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                        options.BackchannelHttpHandler = httpHandler;
                    }

                    options.Authority = "https://sso.localhost";

                    options.CallbackPath = "/signin-oidc";

                    // Todo. What does this do?
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "codidact.com";
                    options.ClientSecret = "foo";
                    options.ResponseType = "code";

                    // Todo. How many times is this executed?
                    options.GetClaimsFromUserInfoEndpoint = true;

                    // Todo. What does this do?
                    options.SaveTokens = true;
                });

            services.AddRouting();

            services.AddRazorPages()
                .AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Todo. Find a better way of doing this.
            //
            // The hostname is sometimes used for redirects.
            app.Use(async (context, next) =>
            {
                context.Request.Host = new Microsoft.AspNetCore.Http.HostString("localhost");
                context.Request.IsHttps = true;
                await next();
            });

            app.UseDeveloperExceptionPage();

            // Todo. Is this required?
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
