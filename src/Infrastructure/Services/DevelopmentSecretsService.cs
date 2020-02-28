using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Codidact.Authentication.Infrastructure.Common.Interfaces;

namespace Codidact.Authentication.Infrastructure.Services
{
    /// <summary>
    /// This service provides secret configuration options directly from the
    /// <see cref="Microsoft.Extensions.Configuration.IConfiguration" /> service.
    /// </summary>
    public class DevelopmentSecretsService : ISecretsService
    {
        private readonly IConfiguration _configuration;

        public DevelopmentSecretsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<string> Get(string key)
        {
            return Task.FromResult(_configuration.GetValue<string>(key));
        }
    }
}
