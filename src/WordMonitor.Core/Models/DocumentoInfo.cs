using WordMonitor.Models;

namespace WordMonitor.Models;

public class DocumentoInfo
{
    public string Caminho { get; set; } = "";

    public DateTime? DataInicioValidade { get; set; }


    public DateTime? DataFimValidade { get; set; }


    public DateTime UltimaModificacao { get; set; }

    public long TamanhoArquivo { get; set; }



    public void CalcularValidade(
        ValidadeConfig config)
    {
        if(!DataInicioValidade.HasValue)
            return;


        DataFimValidade =
            DataInicioValidade.Value
                .AddYears(config.AcrescimoAno)
                .AddMonths(config.AcrescimoMeses)
                .AddDays(config.AcrescimoDias);
    }
}

