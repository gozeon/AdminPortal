using AdminPortal.Options;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MimeKit;

namespace AdminPortal.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly EmailOption _emailSettings;

        public SmtpEmailSender(IOptions<EmailOption> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                _emailSettings.FromName,
                _emailSettings.From));

            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = htmlMessage
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, _emailSettings.EnableSsl);
                await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

        }

    }
}
