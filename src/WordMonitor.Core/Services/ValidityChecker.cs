using WordMonitor.Models;

namespace WordMonitor.Services;

public class ValidityChecker
{
    private readonly int _diasAviso;


    public ValidityChecker(int diasAviso)
    {
        _diasAviso = diasAviso;
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
