using BuildingStore.Models;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace BuildingStore.Services.Patterns.Bridge
{
    public class SmtpEmailSenderBridge : IDocumentSenderBridge
    {
        private readonly EmailSettings settings;

        public SmtpEmailSenderBridge(IOptions<EmailSettings> options)
        {
            settings = options.Value;
        }

        public void Deliver(string docContent, string recipientEmail)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(settings.SenderName, settings.SenderEmail));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = "Деталі вашого замовлення - Z-Build Store";

            message.Body = new TextPart("plain")
            {
                Text = docContent
            };

            using var client = new SmtpClient();
            try
            {
                client.Connect(settings.SmtpServer, settings.Port, SecureSocketOptions.StartTls);
                client.Authenticate(settings.SenderEmail, settings.Password);
                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EMAIL ERROR] Помилка відправки: {ex.Message}");
            }
        }
    }
}
