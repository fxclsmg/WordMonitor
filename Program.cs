using WordMonitor.Notifications;
using WordMonitor.Services;

var config = new ConfigurationBuilder()
    .AddJsonFile(
        "appsettings.json",
        optional: false,
        reloadOnChange: true)
    .AddJsonFile(
        "Config\\notificacoes.json",
        optional: false,
        reloadOnChange: true)
    .AddJsonFile(
        "appsettings.Local.json", 
        optional: true,
        reloadOnChange: true
    )

    .Build();

var pasta = config["Arquivos:Pasta"];
    //@"C:\Users\fxcls\OneDrive\Documentos\docs";

// Notificador
INotifier notifier = new EmailNotifier(config);

// Serviços
var scanner =  new DocumentScanner();

var diasAviso = int.Parse(
        config["DiasAvisoVencimento"] ?? "30"
    );

var checker = new ValidityChecker(
        diasAviso
    );

var builder = new NotificationBuilder(
        config
    );

using var monitor =
    new DocumentMonitorService(
        pasta,
        notifier,
        scanner,
        checker,
        builder
    );

using var expiration =
    new ExpirationService(
        pasta,
        scanner,
        checker,
        builder,
        notifier
    );

await monitor.IniciarAsync();

Console.WriteLine();

Console.WriteLine(
    "Pressione ENTER para finalizar."
);

Console.ReadLine();
