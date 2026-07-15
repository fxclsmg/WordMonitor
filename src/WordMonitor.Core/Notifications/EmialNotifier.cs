using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using WordMonitor.Models;

namespace WordMonitor.Notifications;

public class EmailNotifier : INotifier
{
    private readonly IConfiguration _config;


    public EmailNotifier(
        IConfiguration config)
    {
        _config = config;
    }


    public async Task NotificarAsync(
        Notificacao notificacao)
    {
        var usuario =
            _config["Email:Usuario"];


        var senha =
            _config["Email:Senha"];


        var destino =
            _config["Email:Destino"];



        var email = new MimeMessage();


        email.From.Add(
            new MailboxAddress(
                "WordMonitor",
                usuario!
            )
        );


        email.To.Add(
            new MailboxAddress(
                "Administrador",
                destino!
            )
        );


        email.Subject =
            notificacao.Assunto;


        email.Body =
            new TextPart("plain")
            {
                Text = notificacao.Corpo
            };



        using var smtp = new SmtpClient();


        await smtp.ConnectAsync(
            _config["Email:Servidor"],
            int.Parse(
                _config["Email:Porta"]!
            ),
            MailKit.Security.SecureSocketOptions.StartTls
        );


        await smtp.AuthenticateAsync(
            usuario,
            senha
        );


        await smtp.SendAsync(email);


        await smtp.DisconnectAsync(true);
    }
}