using WordMonitor.Models;
using WordMonitor.Notifications;

namespace WordMonitor.Services;

public class ExpirationService : IDisposable
{
    private readonly string _pasta;

    private readonly DocumentScanner _scanner;

    private readonly ValidityChecker _checker;

    private readonly NotificationBuilder _builder;

    private readonly INotifier _notifier;

    private readonly Timer _timer;

    public ExpirationService(
        string pasta,
        DocumentScanner scanner,
        ValidityChecker checker,
        NotificationBuilder builder,
        INotifier notifier)
    {
        _pasta = pasta;

        _scanner = scanner;

        _checker = checker;

        _builder = builder;

        _notifier = notifier;

        // Executa a primeira verificação após 10 segundos
        // Depois repete a cada 24 horas
        _timer = new Timer(
            VerificarDocumentos,
            null,
            TimeSpan.FromSeconds(10),
            TimeSpan.FromDays(1)
        );
    }

    private async void VerificarDocumentos(
        object? state)
    {
        try
        {
            Console.WriteLine(
                "Verificando vencimentos..."
            );

            var documentos =
                await _scanner.EscanearAsync(
                    _pasta
                );

            foreach(var documento in documentos)
            {
                if(!documento.DataValidade.HasValue)
                    continue;

                var status =
                    _checker.Verificar(
                        documento.DataValidade.Value
                    );

                if(status == StatusDocumento.Valido)
                    continue;

                var notificacao =
                    _builder.CriarVencimento(
                        documento,
                        status
                    );

                await _notifier.NotificarAsync(
                    notificacao
                );

                await Task.Delay(1000);
            }

            Console.WriteLine(
                "Verificação concluída."
            );

        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}
