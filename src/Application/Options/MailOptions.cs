namespace Codidact.Authentication.Application.Options
{
    public class MailOptions
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string SenderName { get; set; }

        public string Sender { get; set; }

        public bool EnableSsl { get; set; }

        public string Password { get; set; }
    }
}
