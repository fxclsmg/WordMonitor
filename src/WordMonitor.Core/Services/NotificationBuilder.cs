using Microsoft.Extensions.Configuration;
using WordMonitor.Models;

namespace WordMonitor.Services;

public class NotificationBuilder
{
    private readonly IConfiguration _config;


    public NotificationBuilder(
        IConfiguration config)
    {
        _config = config;
    }



    public Notificacao CriarVencimento(
        DocumentoInfo documento,
        StatusDocumento status)
    {
        var arquivo =
            Path.GetFileName(documento.Caminho);

        Notificacao notificacaoVencido = new Notificacao();

        if (status == StatusDocumento.ProximoVencimento)
        {
            var dias =
                (documento.DataFimValidade!.Value.Date -
                 DateTime.Now.Date).Days;


            var assunto =
                _config[
                    "Mensagens:DocumentoPertoVencimento:Assunto"
                ];


            var corpo =
                _config[
                    "Mensagens:DocumentoPertoVencimento:Corpo"
                ];

            notificacaoVencido = new Notificacao
            {
                Assunto =
                    assunto!
                    .Replace("{arquivo}", arquivo),

                Corpo =
                    corpo!
                    .Replace("{arquivo}", arquivo)
                    .Replace("{dias}", dias.ToString())
                    .Replace("{data}",
                        documento.DataFimValidade.Value
                        .ToString("dd/MM/yyyy"))
            };
        }

        if (status == StatusDocumento.Vencido)
        {
            var assunto =
                _config[
                    "Mensagens:DocumentoVencido:Assunto"
                ];


            var corpo =
                _config[
                    "Mensagens:DocumentoVencido:Corpo"
                ];

            notificacaoVencido =  new Notificacao
            {
                Assunto =
                    assunto!
                    .Replace("{arquivo}", arquivo),

                Corpo =
                    corpo!
                    .Replace("{arquivo}", arquivo)
                    .Replace("{data}",
                        documento.DataFimValidade!.Value
                        .ToString("dd/MM/yyyy"))
            };
        }
        
        return notificacaoVencido;
    }

    public Notificacao CriarRenovacao(
        DocumentoInfo documento,
        DateTime dataAntiga)
    {
        var arquivo =
            Path.GetFileName(documento.Caminho);



        var assunto =
            _config[
                "Mensagens:DocumentoRenovado:Assunto"
            ];


        var corpo =
            _config[
                "Mensagens:DocumentoRenovado:Corpo"
            ];



        var notificacaoRenovacao = new Notificacao
        {
            Assunto =
                assunto!
                .Replace("{arquivo}", arquivo),


            Corpo =
                corpo!
                .Replace("{arquivo}", arquivo)
                .Replace(
                    "{dataAntiga}",
                    dataAntiga.ToString("dd/MM/yyyy")
                )
                .Replace(
                    "{dataNova}",
                    documento.DataFimValidade!.Value
                    .ToString("dd/MM/yyyy")
                )
        };

        return notificacaoRenovacao;
    }
}
