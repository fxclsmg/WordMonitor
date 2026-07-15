using System.Globalization;
using System.Text.RegularExpressions;

namespace WordMonitor.Services;

public class ParserService
{
    private readonly List<string> _regexes;


    public ParserService(
        List<string> regexes)
    {
        _regexes = regexes;
    }



    public DateTime? ExtrairDataValidade(string texto)
    {

        foreach(var regex in _regexes)
        {


            var match =
                Regex.Match(
                    texto,
                    regex,
                    RegexOptions.IgnoreCase
                );


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

