using WordMonitor.Models;
using WordMonitor.Utils;

namespace WordMonitor.Services;

public class DocumentScanner
{
    private readonly WordReader _reader;

    private readonly ParserService _parser;

    private readonly ValidadeConfig _validadeConfig;



    public DocumentScanner(
        ParserService parser,
        ValidadeConfig validadeConfig)
    {
        _reader = new WordReader();

        _parser = parser;

        _validadeConfig = validadeConfig;
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
                .Where(a =>
                    !Path
                    .GetFileName(a)
                    .StartsWith("~$")
                );



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

