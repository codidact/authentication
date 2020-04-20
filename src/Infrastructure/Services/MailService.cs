using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;
using System.Web;

using Codidact.Authentication.Application.Common.Interfaces;
using Codidact.Authentication.Domain.Entities;
using Codidact.Authentication.Application.Options;

namespace Codidact.Authentication.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private MailOptions _emailConfiguration;
        private ISecretsService _secretsService;
        public MailService(MailOptions emailConfiguration, ISecretsService secretsService)
        {
            _emailConfiguration = emailConfiguration;
            _secretsService = secretsService;
        }
        private async Task SendEmailAsync(ApplicationUser user, string subject, string textMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailConfiguration.SenderName, _emailConfiguration.Sender));
            message.To.Add(new MailboxAddress(user.DisplayName, user.Email));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = textMessage
            };

            using (var emailClient = new SmtpClient())
            {
                await emailClient.ConnectAsync(_emailConfiguration.Host, _emailConfiguration.Port, _emailConfiguration.EnableSsl);

                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                await emailClient.AuthenticateAsync(_emailConfiguration.Sender, await _secretsService.Get("EmailConfiguration:SenderEmailPassword"));

                await emailClient.SendAsync(message);

                await emailClient.DisconnectAsync(true);
            }

        }
        public Task SendResetPassword(ApplicationUser user, string token, string returnUrl)
        {
            var email = HttpUtility.UrlEncode(user.Email);
            return SendEmailAsync(user, "Reset your Password", $"Click here to reset your password <a href='http://localhost:8001/account/reset-password?token={HttpUtility.UrlEncode(token)}&email={email}&returnurl=${HttpUtility.UrlEncode(returnUrl)}'>Reset</a>");
        }
        public Task SendVerificationEmail(ApplicationUser user, string token, string returnUrl)
        {
            var email = HttpUtility.UrlEncode(user.Email);
            return SendEmailAsync(user, "Verify your Email", $"Click here to verify your email <a href='http://localhost:8001/account/email-verification?token={HttpUtility.UrlEncode(token)}&email={email}&returnurl=${HttpUtility.UrlEncode(returnUrl)}'>Verify</a>");
        }
    }
}
