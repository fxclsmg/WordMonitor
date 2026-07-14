namespace WordMonitor.Models;

public class DocumentoInfo
{
    public string Caminho { get; set; } = "";

    public string NomeArquivo => Path.GetFileName(Caminho);

    public DateTime? DataValidade { get; set; }

    public DateTime UltimaModificacao { get; set; }

    public long TamanhoArquivo { get; set; }

}
