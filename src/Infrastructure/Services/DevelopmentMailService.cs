using System.Threading.Tasks;
using System;

using Microsoft.Extensions.Logging;

using Codidact.Authentication.Application.Common.Interfaces;
using Codidact.Authentication.Domain.Entities;

namespace Codidact.Authentication.Infrastructure.Services
{
    public class DevelopmentMailService : IMailService
    {
        private readonly ILogger _logger;

        public DevelopmentMailService(ILogger<DevelopmentMailService> logger)
        {
            _logger = logger;
        }

        public Task SendResetPassword(ApplicationUser user, string token, string returnUrl)
        {
            _logger.LogInformation($"Sending password reset email to {user.Email} with the return url {returnUrl}.");
            return Task.CompletedTask;
        }
        public Task SendVerificationEmail(ApplicationUser user, string token, string returnUrl)
        {
            _logger.LogInformation($"Sending email verfication email to {user.Email} with the return url {returnUrl}.");
            return Task.CompletedTask;
        }
    }
}
