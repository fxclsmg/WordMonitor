using WordMonitor.Models;

namespace WordMonitor.Notifications;

public class LogNotifier : INotifier
{
    public Task NotificarAsync(
        Notificacao notificacao)
    {
        Console.WriteLine();
        Console.WriteLine("========================");
        Console.WriteLine(notificacao.Assunto);
        Console.WriteLine("------------------------");
        Console.WriteLine(notificacao.Corpo);
        Console.WriteLine("========================");
        Console.WriteLine();

        return Task.CompletedTask;
    }
}