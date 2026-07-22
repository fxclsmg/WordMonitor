using MailKit.Net.Smtp;
using MimeKit;
using WordMonitor.Models;
using WordMonitor.Configuration;
using Microsoft.Extensions.Options;


namespace WordMonitor.Notifications;

public class EmailNotifier : INotifier
{

    private readonly IOptionsMonitor<EmailConfig> _options;


    public EmailNotifier(
        IOptionsMonitor<EmailConfig> config)
    {
        _options = config;
    }

    public async Task NotificarAsync(
        Notificacao notificacao)
    {
        var config = _options.CurrentValue;

        var email = new MimeMessage();

        email.From.Add(
            new MailboxAddress(
                "WordMonitor",
                config.Usuario!
            )
        );

        email.To.Add(
            new MailboxAddress(
                "Administrador",
                config.Destinatario!
            )
        );

        email.Subject = notificacao.Assunto;

        email.Body =
            new TextPart("plain")
            {
                Text = notificacao.Corpo
            };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            config.Servidor,
            config.Porta,
            MailKit.Security.SecureSocketOptions.StartTls
        );

        await smtp.AuthenticateAsync(
            config.Usuario,
            config.Senha
        );

        await smtp.SendAsync(email);

        await smtp.DisconnectAsync(true);
    }
}