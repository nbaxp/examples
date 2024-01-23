using MailKit.Net.Smtp;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MimeKit;
using Wta.Application.Identity.Options;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Interfaces;

namespace Wta.Infrastructure.Services;

[Service<IEmailService>]
public class DefaultEmailService(IStringLocalizer stringLocalizer, IOptions<EmailOptions> options) : IEmailService
{
    public void Send(string subject, string body, string toName, string toAddress)
    {
        var host = options.Value.Host;
        var port = options.Value.Port;
        var enableSsl = options.Value.EnableSsl;
        var userName = options.Value.UserName;
        var password = options.Value.Password;
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(stringLocalizer["EmailSenderName"], userName));
        message.To.Add(new MailboxAddress(toName, toAddress));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient
        {
            ServerCertificateValidationCallback = (s, c, h, e) => true
        };

        client.Connect(host, port, enableSsl);
        try
        {
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(userName, password);
            client.Send(message);
        }
        catch
        {
            client.Disconnect(true);
            throw;
        }
    }
}
