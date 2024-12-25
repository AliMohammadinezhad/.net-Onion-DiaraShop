using MailKit.Net.Smtp;
using MimeKit;

namespace Framework.Application.Email
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string title, string messageBody, string destination)
        {
            var message = new MimeMessage();

            var from = new MailboxAddress("Atriya", "test@atriya.com");
            message.From.Add(from);

            var to = new MailboxAddress("User", destination);
            message.To.Add(to);

            message.Subject = title;
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<h1>{messageBody}</h1>",
            };

            message.Body = bodyBuilder.ToMessageBody();

            var client = new SmtpClient();
            client.Connect("host address", 25, false);
            client.Authenticate("username", "password");
            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }
    }
}