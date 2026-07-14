namespace WordMonitor.Notifications;

using WordMonitor.Models;

public interface INotifier
{
    Task NotificarAsync(Notificacao notificacao);
}