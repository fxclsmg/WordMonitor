namespace WordMonitor.Configuration;
public class MensagensConfig
{
    public MensagemConfig DocumentoPertoVencimento { get; set; } = new();

    public MensagemConfig DocumentoVencido { get; set; } = new();

    public MensagemConfig DocumentoRenovado { get; set; } = new();
}

public class MensagemConfig
{
    public string Assunto { get; set; } = "";

    public string Corpo { get; set; } = "";
}
