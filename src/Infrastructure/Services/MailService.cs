using System;
using System.Threading.Tasks;
using System.Web;

using Microsoft.Extensions.Options;

using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

using Codidact.Authentication.Application.Common.Interfaces;
using Codidact.Authentication.Domain.Entities;
using Codidact.Authentication.Application.Options;

namespace Codidact.Authentication.Infrastructure.Services
{
    public class MailService : IMailService, IDisposable
    {
        private readonly MailOptions _options;
        private readonly Task<SmtpClient> _client;

        public MailService(IOptions<MailOptions> options)
        {
            _options = options.Value;

            _client = Task.Run(async () =>
            {
                var client = new SmtpClient();

                client.AuthenticationMechanisms.Remove("XOAUTH2");

                await client.ConnectAsync(_options.Host, _options.Port, _options.EnableSsl);
                await client.AuthenticateAsync(_options.Sender, _options.Password);

                return client;
            });
        }

        public void Dispose()
        {
            _client.GetAwaiter().GetResult().Disconnect(true);
        }

        public async Task SendEmailAsync(ApplicationUser user, string subject, string textMessage)
        {
            var html = new TextPart(TextFormat.Html) { Text = textMessage };

            var message = new MimeMessage
            {
                From = { new MailboxAddress(_options.SenderName, _options.Sender) },
                To = { new MailboxAddress(user.DisplayName, user.Email) },
                Subject = subject,
                Body = html,
            };

            var client = await _client;
            await client.SendAsync(message);
        }

        public async Task SendResetPassword(ApplicationUser user, string token, string returnUrl)
        {
            var email = HttpUtility.UrlEncode(user.Email);
            await SendEmailAsync(user, "Reset your Password", $"Click here to reset your password <a href='http://localhost:8001/account/reset-password?token={HttpUtility.UrlEncode(token)}&email={email}&returnurl=${HttpUtility.UrlEncode(returnUrl)}'>Test</a>");
        }
    }
}
