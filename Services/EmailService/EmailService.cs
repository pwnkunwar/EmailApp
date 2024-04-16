using EmailApp.Models;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace EmailApp.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration= configuration;
        }
        public void SendEmail(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["SMTPConnectionStrings:EmailUserName"]));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_configuration["SMTPConnectionStrings:EmailHost"],587 , SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration["SMTPConnectionStrings:EmailUserName"], _configuration["SMTPConnectionStrings:EmailPassword"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
