using WordMonitor.Models;
using WordMonitor.Configuration;
using Microsoft.Extensions.Options;

namespace WordMonitor.Services;

public class ValidityChecker
{
    private readonly int _diasAviso;


    public ValidityChecker(IOptions<ValidadeConfig> options)
    {
        _diasAviso = options.Value.DiasAviso;
    }


    public StatusDocumento Verificar(DateTime validade)
    {
        var hoje = DateTime.Now.Date;


        if (validade < hoje)
            return StatusDocumento.Vencido;


        if ((validade - hoje).Days <= _diasAviso)
            return StatusDocumento.ProximoVencimento;


        return StatusDocumento.Valido;
    }
}
