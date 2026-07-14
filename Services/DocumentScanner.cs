using WordMonitor.Models;
using WordMonitor.Utils;

namespace WordMonitor.Services;

public class DocumentScanner
{
    private readonly WordReader _reader = new();
    private readonly ParserService _parser = new();


    // Lê todos os documentos da pasta
    public async Task<List<DocumentoInfo>> EscanearAsync(string pasta)
    {
        List<DocumentoInfo> documentos = new();


        var arquivos = Directory
            .GetFiles(pasta, "*.docx")
            .Where(a => !Path.GetFileName(a).StartsWith("~$"));


        foreach (var arquivo in arquivos)
        {
            var documento =
                await LerDocumentoAsync(arquivo);


            if (documento != null)
            {
                documentos.Add(documento);
            }
        }


        return documentos;
    }



    // Lê apenas um documento
    public async Task<DocumentoInfo?> LerDocumentoAsync(string arquivo)
    {
        Console.WriteLine(
            $"Lendo {Path.GetFileName(arquivo)}..."
        );


        if (!await FileHelper.AguardarArquivoDisponivel(arquivo))
        {
            Console.WriteLine(
                $"Não foi possível abrir {arquivo}"
            );

            return null;
        }


        try
        {
            string texto =
                _reader.LerTexto(arquivo);


            var info =
                new FileInfo(arquivo);



            return new DocumentoInfo
            {
                Caminho = arquivo,

                DataValidade =
                    _parser.ExtrairDataValidade(texto),

                UltimaModificacao =
                    info.LastWriteTime,

                TamanhoArquivo =
                    info.Length
            };

        }
        catch(Exception ex)
        {
            Console.WriteLine(
                $"Erro ao processar {arquivo}"
            );

            Console.WriteLine(
                ex.Message
            );

            return null;
        }
    }
}
