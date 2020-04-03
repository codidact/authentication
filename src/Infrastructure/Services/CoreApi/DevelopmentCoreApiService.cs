using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Codidact.Authentication.Application.Common.Interfaces;
using Codidact.Authentication.Domain.Common;

namespace Codidact.Authentication.Infrastructure.Services
{
    public class DevelopmentCoreApiService : ICoreApiService
    {
        private readonly ILogger<DevelopmentCoreApiService> _logger;

        public DevelopmentCoreApiService(ILogger<DevelopmentCoreApiService> logger)
        {
            _logger = logger;
        }

        public Task<EntityResult> CreateMemberAsync(string url, string displayName, long userId)
        {
            _logger.LogInformation("Create Member for the user id");

            // TODO: Implement a real service that sends Core Codidact API a request

            _logger.LogInformation("Member Created.");

            return Task.FromResult(new EntityResult(true));
        }
    }
}
