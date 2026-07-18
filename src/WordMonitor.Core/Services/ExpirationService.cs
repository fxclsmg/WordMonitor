using Microsoft.Extensions.Options;
using WordMonitor.Configuration;
using WordMonitor.Models;
using WordMonitor.Notifications;

namespace WordMonitor.Services;

public class ExpirationService
{
    private readonly MonitorConfig _config;

    private readonly DocumentScanner _scanner;

    private readonly ValidityChecker _checker;

    private readonly NotificationBuilder _builder;

    private readonly INotifier _notifier;


    public ExpirationService(
        IOptions<MonitorConfig> options,
        DocumentScanner scanner,
        ValidityChecker checker,
        NotificationBuilder builder,
        INotifier notifier)
    {
        _config = options.Value;
        _scanner = scanner;
        _checker = checker;
        _builder = builder;
        _notifier = notifier;
    }

    public async Task VerificarAsync(
        CancellationToken token)
    {
        try
        {
            Console.WriteLine(
                $"{DateTime.Now}; Iniciando verificação de validade..."
            );


            var documentos =
                await _scanner.EscanearAsync(
                    _config.Pasta
                );


            foreach(var documento in documentos)
            {
                token.ThrowIfCancellationRequested();


                if (!documento.DataFimValidade.HasValue)
                {
                    continue;
                }


                var status =
                    _checker.Verificar(
                        documento.DataFimValidade.Value
                    );


                if(status == StatusDocumento.Valido)
                {
                    continue;
                }


                var notificacao =
                    _builder.CriarVencimento(
                        documento,
                        status
                    );


                await _notifier.NotificarAsync(
                    notificacao
                );
            }


            Console.WriteLine(
                $"{DateTime.Now}; Verificação concluída."
            );
        }
        catch(OperationCanceledException)
        {
            Console.WriteLine(
                $"{DateTime.Now}; Verificação cancelada."
            );
        }
        catch(Exception ex)
        {
            Console.WriteLine(
                $"{DateTime.Now}; Erro: {ex.Message}"
            );
        }
    }
}
