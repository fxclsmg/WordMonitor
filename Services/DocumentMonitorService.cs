using WordMonitor.Models;
using WordMonitor.Notifications;

namespace WordMonitor.Services;

public class DocumentMonitorService : IDisposable
{
    private readonly string _pasta;

    private readonly DocumentScanner _scanner;

    private readonly ValidityChecker _checker;

    private readonly NotificationBuilder _builder;

    private readonly INotifier _notifier;

    private readonly FileSystemWatcher _watcher;

    private readonly Dictionary<string, DocumentoInfo> _documentos =
        new();

    private readonly SemaphoreSlim _lock = new(1, 1);



    public DocumentMonitorService(
        string pasta,
        INotifier notifier,
        DocumentScanner scanner,
        ValidityChecker checker,
        NotificationBuilder builder)
    {
        _pasta = pasta;

        _notifier = notifier;

        _scanner = scanner;

        _checker = checker;

        _builder = builder;


        _watcher =
            new FileSystemWatcher(
                _pasta,
                "*.docx"
            );


        _watcher.IncludeSubdirectories = false;


        _watcher.NotifyFilter =
            NotifyFilters.LastWrite |
            NotifyFilters.FileName |
            NotifyFilters.Size;


        _watcher.Changed += OnArquivoAlterado;
        _watcher.Created += OnArquivoAlterado;
        _watcher.Renamed += OnArquivoAlterado;
        _watcher.Deleted += OnArquivoRemovido;
    }



    public async Task IniciarAsync()
    {
        Console.WriteLine(
            "Carregando documentos..."
        );


        var documentos =
            await _scanner.EscanearAsync(_pasta);


        foreach(var documento in documentos)
        {
            _documentos[documento.Caminho] =
                documento;
        }


        Console.WriteLine(
            $"{_documentos.Count} documentos carregados."
        );


        _watcher.EnableRaisingEvents = true;


        Console.WriteLine(
            "Monitor iniciado."
        );
    }





    private async void OnArquivoAlterado(
        object? sender,
        FileSystemEventArgs e)
    {
        if (Path.GetFileName(e.FullPath)
            .StartsWith("~$"))
        {
            return;
        }


        await _lock.WaitAsync();


        try
        {
            Console.WriteLine();

            Console.WriteLine(
                $"Alteração detectada: {e.Name}"
            );


            await Task.Delay(1000);



            var documentoNovo =
                await _scanner.LerDocumentoAsync(
                    e.FullPath
                );


            if(documentoNovo == null)
            {
                return;
            }



            _documentos.TryGetValue(
                e.FullPath,
                out var documentoAntigo
            );



            // Documento novo criado
            if(documentoAntigo == null)
            {
                _documentos[e.FullPath] =
                    documentoNovo;

                return;
            }



            // Verifica renovação
            if(documentoNovo.DataValidade >
               documentoAntigo.DataValidade)
            {
                var notificacao =
                    _builder.CriarRenovacao(
                        documentoNovo,
                        documentoAntigo.DataValidade!.Value
                    );


                await _notifier.NotificarAsync(
                    notificacao
                );
            }



            // Verifica vencimento
            if(documentoNovo.DataValidade.HasValue)
            {
                var status =
                    _checker.Verificar(
                        documentoNovo.DataValidade.Value
                    );


                if(status != StatusDocumento.Valido)
                {
                    var notificacao =
                        _builder.CriarVencimento(
                            documentoNovo,
                            status
                        );


                    await _notifier.NotificarAsync(
                        notificacao
                    );
                }
            }



            // Atualiza estado
            _documentos[e.FullPath] =
                documentoNovo;

        }
        catch(Exception ex)
        {
            Console.WriteLine(
                ex.Message
            );
        }
        finally
        {
            _lock.Release();
        }
    }





    private void OnArquivoRemovido(
        object? sender,
        FileSystemEventArgs e)
    {
        if(_documentos.ContainsKey(e.FullPath))
        {
            _documentos.Remove(
                e.FullPath
            );
        }


        Console.WriteLine(
            $"Documento removido: {e.Name}"
        );
    }





    public void Dispose()
    {
        _watcher.Dispose();

        _lock.Dispose();
    }
}
