using Codidact.Authentication.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Codidact.Authentication.Infrastructure.Services
{
    public class DevelopmentCoreApiService : ICoreApiService
    {
        private readonly ILogger<DevelopmentCoreApiService> _logger;
     
        public DevelopmentCoreApiService(ILogger<DevelopmentCoreApiService> logger)
        {
            _logger = logger;
        }

        public Task<bool> CreateMember(string displayName, long userId)
        {
            _logger.LogInformation("Create Member for the user id");

            // TODO: Implement a real service that sends Core Codidact API a request

            _logger.LogInformation("Member Created.");

            return Task.FromResult(true);
        }
    }
}
