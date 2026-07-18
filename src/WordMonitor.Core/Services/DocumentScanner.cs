using WordMonitor.Models;
using WordMonitor.Utils;
using WordMonitor.Configuration;
using Microsoft.Extensions.Options;

namespace WordMonitor.Services;

public class DocumentScanner
{
    private readonly WordReader _reader;

    private readonly ParserService _parser;

    private readonly ValidadeConfig _validadeConfig;

    private readonly MonitorConfig _monitorConfig;



    public DocumentScanner(
        ParserService parser,
        IOptions<ValidadeConfig> validadeConfig,
        IOptions<MonitorConfig> monitorConfig)
    {
        _reader = new WordReader();

        _parser = parser;

        _validadeConfig = validadeConfig.Value;

        _monitorConfig = monitorConfig.Value;
    }


    // Lê todos os documentos da pasta e subpastas
    public async Task<List<DocumentoInfo>> EscanearAsync(
        string pasta)
    {
        List<DocumentoInfo> documentos = new();



        var arquivos =
            Directory
                .GetFiles(
                    pasta,
                    "*.docx",
                    SearchOption.AllDirectories
                )
                .Where(ArquivoPermitido);



        foreach(var arquivo in arquivos)
        {
            var documento =
                await LerDocumentoAsync(
                    arquivo
                );


            if(documento != null)
            {
                documentos.Add(
                    documento
                );
            }
        }



        return documentos;
    }





    // Verifica se o arquivo deve ser processado
    private bool ArquivoPermitido(
        string arquivo)
    {
        var nomeArquivo =
            Path.GetFileName(
                arquivo
            );


        // Ignora arquivos temporários do Word
        if(nomeArquivo.StartsWith("~"))
        {
            return false;
        }



        // Se não houver prefixos configurados,
        // considera todos permitidos
        if(_monitorConfig.PrefixosPermitidos.Count == 0)
        {
            return true;
        }



        return _monitorConfig
            .PrefixosPermitidos
            .Any(prefixo =>
                nomeArquivo.StartsWith(
                    prefixo,
                    StringComparison.OrdinalIgnoreCase
                )
            );
    }





    // Lê um documento individual
    public async Task<DocumentoInfo?> LerDocumentoAsync(
        string arquivo)
    {
        Console.WriteLine(
            $"{DateTime.Now}; Lendo {Path.GetFileName(arquivo)}..."
        );



        if(!await FileHelper.AguardarArquivoDisponivel(arquivo))
        {
            Console.WriteLine(
                $"{DateTime.Now}; Não foi possível abrir {arquivo}"
            );

            return null;
        }



        try
        {
            string texto =
                _reader.LerTexto(
                    arquivo
                );



            var info =
                new FileInfo(
                    arquivo
                );



            var documento =
                new DocumentoInfo
                {
                    Caminho = arquivo,


                    DataInicioValidade =
                        _parser.ExtrairDataValidade(
                            texto
                        ),


                    UltimaModificacao =
                        info.LastWriteTime,


                    TamanhoArquivo =
                        info.Length
                };



            documento.CalcularValidade(
                _validadeConfig
            );



            return documento;
        }
        catch(Exception ex)
        {
            Console.WriteLine(
                $"{DateTime.Now}; Erro ao processar {arquivo}"
            );


            Console.WriteLine(
                ex.Message
            );


            return null;
        }
    }
}

