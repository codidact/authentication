using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
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
                        { "IdentityServer:Clients:0:ClientSecrets:0:Value", "testsecret".Sha256() },
                        { "IdentityServer:Clients:0:AllowedGrantTypes:0", "client_credentials" },
                        { "IdentityServer:Clients:0:AllowedScopes:0", "openid" },
                        { "ConnectionStrings:Authentication", $"Data Source={Path.GetTempFileName()}" },
                    });
                })
                .ConfigureWebHost(webhost =>
                {
                    webhost.UseEnvironment(Environments.Development);
                    webhost.UseTestServer();
                    webhost.UseStartup<Startup>();
                })
                .Start();

            _http = host.GetTestClient();
        }

        [Fact]
        public async Task GetOpenidConfiguration()
        {
            var response = await _http.GetDiscoveryDocumentAsync();

            Assert.False(response.IsError);
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

            Assert.True(response.IsError);
        }
    }
}
