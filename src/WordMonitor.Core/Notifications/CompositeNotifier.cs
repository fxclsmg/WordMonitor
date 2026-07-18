using Microsoft.Extensions.Options;
using WordMonitor.Configuration;
using WordMonitor.Models;

namespace WordMonitor.Notifications;

public class CompositeNotifier : INotifier
{
    private readonly LogNotifier _logNotifier;

    private readonly EmailNotifier _emailNotifier;

    private readonly IOptionsMonitor<NotificarConfig> _options;



    public CompositeNotifier(
        LogNotifier logNotifier,
        EmailNotifier emailNotifier,
        IOptionsMonitor<NotificarConfig> options)
    {
        _logNotifier = logNotifier;

        _emailNotifier = emailNotifier;

        _options = options;
    }



    public async Task NotificarAsync(
        Notificacao notificacao)
    {
        var config =
            _options.CurrentValue;


        if(config.Log)
        {
            await _logNotifier.NotificarAsync(
                notificacao
            );
        }

        if(config.Email)
        {
            await _emailNotifier.NotificarAsync(
                notificacao
            );
        }
    }
}
