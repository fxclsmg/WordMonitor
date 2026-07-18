namespace WordMonitor.Configuration;

public class ValidadeConfig
{
    public int DiasAviso { get; set; }

    public int AcrescimoAno { get; set; }

    public int AcrescimoMeses { get; set; }

    public int AcrescimoDias { get; set; }

    public List<PadraoRegex> Padroes { get; set; } = new();
}


public class PadraoRegex
{
    public string Nome { get; set; } = "";

    public string Regex { get; set; } = "";
}

