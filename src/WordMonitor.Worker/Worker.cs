using WordMonitor.Services;

namespace WordMonitor.Worker;

public class Worker : BackgroundService
{
    private readonly DocumentMonitorService _monitor;

    private readonly ExpirationService _expirationService;

    private readonly IConfiguration _configuration;



    public Worker(
        DocumentMonitorService monitor,
        ExpirationService expirationService,
        IConfiguration configuration)
    {
        _monitor = monitor;
        _expirationService = expirationService;
        _configuration = configuration;
    }



    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        Console.WriteLine(
            $"{DateTime.Now}; WordMonitor iniciado."
        );


        await _monitor.IniciarAsync();


        await ExecutarVerificacaoDiariaAsync(
            stoppingToken
        );
    }




    private async Task ExecutarVerificacaoDiariaAsync(
        CancellationToken token)
    {
        var horario =
            _configuration["Verificacao:Horario"];


        if(!TimeSpan.TryParse(
            horario,
            out var hora))
        {
            throw new Exception(
                "Horário inválido."
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

