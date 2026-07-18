using Microsoft.Extensions.Options;
using WordMonitor.Configuration;
using WordMonitor.Models;

namespace WordMonitor.Services;

public class NotificationBuilder
{
    private readonly MensagensConfig _config;

    public NotificationBuilder(
        IOptions<MensagensConfig> options)
    {
        _config = options.Value;
    }

    public Notificacao CriarVencimento(
        DocumentoInfo documento,
        StatusDocumento status)
    {
        var arquivo =
            Path.GetFileName(documento.Caminho);

        Notificacao notificacao = new();

        if (status == StatusDocumento.ProximoVencimento)
        {
            var dias =
                (documento.DataFimValidade!.Value.Date -
                 DateTime.Now.Date).Days;

            notificacao = new Notificacao
            {
                Assunto =
                    _config.DocumentoPertoVencimento.Assunto
                        .Replace("{arquivo}", arquivo),

                Corpo =
                    _config.DocumentoPertoVencimento.Corpo
                        .Replace("{arquivo}", arquivo)
                        .Replace("{dias}", dias.ToString())
                        .Replace(
                            "{data}",
                            documento.DataFimValidade.Value
                                .ToString("dd/MM/yyyy"))
            };
        }
        else if (status == StatusDocumento.Vencido)
        {
            notificacao = new Notificacao
            {
                Assunto =
                    _config.DocumentoVencido.Assunto
                        .Replace("{arquivo}", arquivo),

                Corpo =
                    _config.DocumentoVencido.Corpo
                        .Replace("{arquivo}", arquivo)
                        .Replace(
                            "{data}",
                            documento.DataFimValidade!.Value
                                .ToString("dd/MM/yyyy"))
            };
        }

        return notificacao;
    }

    public Notificacao CriarRenovacao(
        DocumentoInfo documento,
        DateTime dataAntiga)
    {
        var arquivo =
            Path.GetFileName(documento.Caminho);

        return new Notificacao
        {
            Assunto =
                _config.DocumentoRenovado.Assunto
                    .Replace("{arquivo}", arquivo),

            Corpo =
                _config.DocumentoRenovado.Corpo
                    .Replace("{arquivo}", arquivo)
                    .Replace(
                        "{dataAntiga}",
                        dataAntiga.ToString("dd/MM/yyyy"))
                    .Replace(
                        "{dataNova}",
                        documento.DataFimValidade!.Value
                            .ToString("dd/MM/yyyy"))
        };
    }
}
