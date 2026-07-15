using WordMonitor.Models;
using WordMonitor.Notifications;

namespace WordMonitor.Services;

public class ExpirationService
{
    private readonly string _pasta;

    private readonly DocumentScanner _scanner;

    private readonly ValidityChecker _checker;

    private readonly NotificationBuilder _builder;

    private readonly INotifier _notifier;


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
    }


    public async Task VerificarAsync(
        CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"{DateTime.Now}; Verificando vencimentos..."
        );


        var documentos =
            await _scanner.EscanearAsync(_pasta);



        foreach(var documento in documentos)
        {
            if(!documento.DataFimValidade.HasValue)
                continue;


            var status =
                _checker.Verificar(
                    documento.DataFimValidade!.Value
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
        }


        Console.WriteLine(
            $"{DateTime.Now}; {documentos.Count} documentos verificados."
        );
    }
}

