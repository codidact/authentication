using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;
using System.Web;
using System;

using Codidact.Authentication.Infrastructure.Common.Interfaces;
using Codidact.Authentication.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Codidact.Authentication.Infrastructure.Services
{
    public class EmailSender : IEmailSender<EmailSettings>
    {
        private EmailSettings _emailConfiguration;
        private IServiceProvider _secretsService;
        public EmailSender(EmailSettings emailConfiguration, IServiceProvider secretsService)
        {
            _emailConfiguration = emailConfiguration;
            _secretsService = secretsService;
        }
        public async Task SendEmailAsync(string email, string subject, string textMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailConfiguration.SenderName, _emailConfiguration.Sender));
            message.To.Add(new MailboxAddress("Username", email));
            message.Subject = subject;

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = textMessage
            };
            var secrets = _secretsService.GetService<ISecretsService>();
            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect(_emailConfiguration.Host, _emailConfiguration.Port, _emailConfiguration.EnableSsl);

                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(_emailConfiguration.Sender, secrets.Get("EmailConfiguration:SenderEmailPassword").GetAwaiter().GetResult());

                await emailClient.SendAsync(message);

                emailClient.Disconnect(true);
            }

        }
        public async Task SendResetPassword(string email, string token, string returnUrl)
        {
            await SendEmailAsync(email, "Reset your Password", $"Click here to reset your password <a href='http://localhost:8001/account/reset-password?token={HttpUtility.UrlEncode(token)}&email={email}&returnurl=${returnUrl}'>Test</a>");
        }
    }
}
