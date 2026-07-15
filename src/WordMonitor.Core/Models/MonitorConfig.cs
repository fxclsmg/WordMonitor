namespace WordMonitor.Models;

public class MonitorConfig
{
    public string Pasta { get; set; } = "";

    public List<string> PrefixosPermitidos { get; set; } = new();
}

