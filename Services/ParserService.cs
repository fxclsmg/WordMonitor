using System.Text.RegularExpressions;

namespace WordMonitor.Services;

public class ParserService
{
    public DateTime? ExtrairDataValidade(string texto)
    {
        var regex = new Regex(
            //@"Data\s+de\s+validade:\s*(\d{2}/\d{2}/\d{4})",
            @"Data\s*de\s*validade[\s:\r\n]*(\d{2}/\d{2}/\d{4})",
            RegexOptions.IgnoreCase);

        var match = regex.Match(texto);

        if (!match.Success)
            return null;

        if (DateTime.TryParse(match.Groups[1].Value,
            out DateTime data))
            return data;

        return null;
    }
}
