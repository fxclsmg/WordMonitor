namespace WordMonitor.Configuration;

public class EmailConfig
{
    public string Servidor { get; set; } = "";

    public int Porta { get; set; }

    public string Usuario { get; set; } = "";

    public string Senha { get; set; } = "";

    public string Destinatario { get; set; } = "";
}
