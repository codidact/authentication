using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;
using System.Web;
using System;

using Codidact.Authentication.Application.Common.Interfaces;
using Codidact.Authentication.Domain.Entities;
using Codidact.Authentication.Application.Options;

using Microsoft.Extensions.DependencyInjection;

namespace Codidact.Authentication.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private MailOptions _emailConfiguration;
        private IServiceProvider _secretsService;
        public MailService(MailOptions emailConfiguration, IServiceProvider secretsService)
        {
            _emailConfiguration = emailConfiguration;
            _secretsService = secretsService;
        }
        public async Task SendEmailAsync(ApplicationUser user, string subject, string textMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailConfiguration.SenderName, _emailConfiguration.Sender));
            message.To.Add(new MailboxAddress(user.UserName, user.Email));
            message.Subject = subject;

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = textMessage
            };
            var secrets = _secretsService.GetService<ISecretsService>();

            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect(_emailConfiguration.Host, _emailConfiguration.Port, _emailConfiguration.EnableSsl);

                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(_emailConfiguration.Sender, secrets.Get("EmailConfiguration:SenderEmailPassword").GetAwaiter().GetResult());

                await emailClient.SendAsync(message);

                emailClient.Disconnect(true);
            }

        }
        public async Task SendResetPassword(ApplicationUser user, string token, string returnUrl)
        {
            var email = HttpUtility.UrlEncode(user.Email);
            await SendEmailAsync(user, "Reset your Password", $"Click here to reset your password <a href='http://localhost:8001/account/reset-password?token={HttpUtility.UrlEncode(token)}&email={HttpUtility.UrlEncode(email)}&returnurl=${HttpUtility.UrlEncode(returnUrl)}'>Test</a>");
        }
    }
}
