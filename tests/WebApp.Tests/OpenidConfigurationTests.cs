using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

using IdentityModel.Client;
using IdentityServer4.Models;

using Xunit;

namespace Codidact.Authentication.WebApp.Tests
{
    public class OpenidConfigurationTests
    {
        private readonly HttpClient _http;

        public OpenidConfigurationTests()
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configuration =>
                {
                    configuration.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "IdentityServer:Clients:0:ClientId", "testclient" },
                        // Note. This is not the SHA256 value of 'testsecret' the function name is
                        // misleading. Instead you get a string that contains the hash and some metadata.
                        { "IdentityServer:Clients:0:ClientSecrets:0:Value", "testsecret".Sha256() },
                        { "IdentityServer:Clients:0:AllowedGrantTypes:0", "client_credentials" },
                        { "IdentityServer:Clients:0:AllowedScopes:0", "openid" },
                    });
                })
                .ConfigureWebHost(webhost =>
                {
                    webhost.UseEnvironment("Testing");
                    webhost.UseTestServer();
                    webhost.UseStartup<Startup>();
                })
                .Start();

            _http = host.GetTestClient();
        }

        [Fact]
        public async Task GetOpenidConfiguration()
        {
            var response = await _http.GetAsync("/.well-known/openid-configuration");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task RequestWithValidCredentials()
        {
            var client = new TokenClient(_http, new TokenClientOptions
            {
                Address = "http://localhost/connect/token",
                ClientId = "testclient",
                ClientSecret = "testsecret",
            });

            var response = await client.RequestClientCredentialsTokenAsync();

            // Note. This is not a good indicator of a successful token request. There are a
            // lot of errors that aren't covered by this. I would go a step further, stating that
            // it appears to be random which errors are considered errors.
            Assert.False(response.IsError);
        }

        [Fact]
        public async Task RequestWithInvalidClientCredentials()
        {
            var client = new TokenClient(_http, new TokenClientOptions
            {
                Address = "http://localhost/connect/token",
                ClientId = "testclient",
                ClientSecret = "incorrect",
            });

            var response = await client.RequestClientCredentialsTokenAsync();

            // Note. This is not a good indicator of a successful token request. There are a
            // lot of errors that aren't covered by this. I would go a step further, stating that
            // it appears to be random which errors are considered errors.
            Assert.True(response.IsError);
        }
    }
}
