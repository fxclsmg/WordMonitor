using WordMonitor.Services;
using Microsoft.Extensions.Options;
using WordMonitor.Configuration;

namespace WordMonitor.Worker;

public class Worker : BackgroundService
{
    private readonly DocumentMonitorService _monitor;

    private readonly ExpirationService _expirationService;

    private readonly VerificacaoConfig _config;

    public Worker(
        DocumentMonitorService monitor,
        ExpirationService expirationService,
        IOptions<VerificacaoConfig> options)
    {
        _monitor = monitor;
        _expirationService = expirationService;
        _config = options.Value;
    }


    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        Console.WriteLine(
            $"{DateTime.Now}; WordMonitor iniciado."
        );


        await _monitor.IniciarAsync();


        try
        {
            await ExecutarVerificacaoAsync(
                stoppingToken
            );
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine(
                $"{DateTime.Now}; WordMonitor finalizado."
            );
        }
    }




    private async Task ExecutarVerificacaoAsync(
        CancellationToken token)
    {

        var horario =_config.Horario;


       if(!TimeSpan.TryParse(
            horario,
            out var hora))
        {
            hora = new TimeSpan(15, 0, 0);

            Console.WriteLine(
                $"{DateTime.Now}; Horário inválido. Usando 15:00."
            );
        }



        while(!token.IsCancellationRequested)
        {
            var agora =
                DateTime.Now;


            var proxima =
                agora.Date.Add(hora);


            if(proxima <= agora)
            {
                proxima =
                    proxima.AddDays(1);
            }


            var espera =
                proxima - agora;



            Console.WriteLine(
                $"{DateTime.Now}; Próxima verificação: {proxima}"
            );



            await Task.Delay(
                espera,
                token
            );


            await _expirationService.VerificarAsync(
                token
            );
        }
    }
}

