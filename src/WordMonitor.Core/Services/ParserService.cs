using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using WordMonitor.Configuration;

namespace WordMonitor.Services;

public class ParserService
{

    private readonly IOptionsMonitor<ValidadeConfig> _options;


    public ParserService(
        IOptionsMonitor<ValidadeConfig> config)
    {
        _options = config;
    }

    public DateTime? ExtrairDataValidade(string texto)
    {

        var config = _options.CurrentValue;

        foreach(var padrao in config.Padroes)
        {


            var match = Regex.Match(
                texto,
                padrao.Regex,
                RegexOptions.IgnoreCase);


            if(!match.Success)
                continue;


            var valor =
                match.Groups[1].Value;


            if(DateTime.TryParse(
                valor,
                CultureInfo.GetCultureInfo("pt-BR"),
                DateTimeStyles.None,
                out var data))
            {
                return data;
            } else
            {
                valor = match.Groups[1].Value + "/" +
                    match.Groups[2].Value + "/" +
                    match.Groups[3].Value;

                    if(DateTime.TryParse(
                        valor,
                        CultureInfo.GetCultureInfo("pt-BR"),
                        DateTimeStyles.None,
                        out var data2))
                    {
                        return data2;
                    }
            }
        }


        return null;
    }
}

