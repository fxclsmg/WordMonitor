using WordMonitor.Notifications;
using WordMonitor.Services;
using WordMonitor.Worker;
using WordMonitor.Models;


var builder = Host.CreateApplicationBuilder(args);


builder.Configuration.AddJsonFile(
    "appsettings.Local.json",
    optional: true,
    reloadOnChange: true
);



// ===============================
// Parser
// ===============================

builder.Services.AddSingleton<ParserService>(
serviceProvider =>
{
    var configuration =
        serviceProvider.GetRequiredService<IConfiguration>();


    var regexes =
        configuration
            .GetSection("Validade:Padroes")
            .GetChildren()
            .Select(x => x["Regex"]!)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();


    return new ParserService(
        regexes
    );
});




// ===============================
// Scanner
// ===============================

builder.Services.AddSingleton<DocumentScanner>();



// ===============================
// ValdiadeConfig
// ===============================
builder.Services.AddSingleton<ValidadeConfig>(
serviceProvider =>
{
    var configuration =
        serviceProvider.GetRequiredService<IConfiguration>();

    var config =
        new ValidadeConfig();

    configuration
        .GetSection("Validade")
        .Bind(config);


    return config;
});




// ===============================
// MonitorConfig
// ===============================


builder.Services.AddSingleton<MonitorConfig>(
serviceProvider =>
{
    var configuration =
        serviceProvider.GetRequiredService<IConfiguration>();


    var config =
        new MonitorConfig();


    configuration
        .GetSection("Monitor")
        .Bind(config);


    return config;
});



// ===============================
// Notificações
// ===============================

builder.Services.AddSingleton<INotifier, LogNotifier>();

builder.Services.AddSingleton<NotificationBuilder>();




// ===============================
// Validade
// ===============================

builder.Services.AddSingleton<ValidityChecker>(
serviceProvider =>
{
    var configuration =
        serviceProvider.GetRequiredService<IConfiguration>();


    var diasAviso =
        configuration.GetValue<int>(
            "Validade:DiasAviso"
        );


    return new ValidityChecker(
        diasAviso
    );
});




// ===============================
// Monitor de arquivos
// ===============================

builder.Services.AddSingleton<DocumentMonitorService>(
serviceProvider =>
{
    var configuration =
        serviceProvider.GetRequiredService<IConfiguration>();


    var pasta =
        configuration["Monitor:Pasta"];



    return new DocumentMonitorService(
        pasta!,
        serviceProvider.GetRequiredService<INotifier>(),
        serviceProvider.GetRequiredService<DocumentScanner>(),
        serviceProvider.GetRequiredService<ValidityChecker>(),
        serviceProvider.GetRequiredService<NotificationBuilder>()
    );
});




// ===============================
// Verificação diária
// ===============================

builder.Services.AddSingleton<ExpirationService>(
serviceProvider =>
{
    var configuration =
        serviceProvider.GetRequiredService<IConfiguration>();


    var pasta =
        configuration["Monitor:Pasta"];



    return new ExpirationService(
        pasta!,
        serviceProvider.GetRequiredService<DocumentScanner>(),
        serviceProvider.GetRequiredService<ValidityChecker>(),
        serviceProvider.GetRequiredService<NotificationBuilder>(),
        serviceProvider.GetRequiredService<INotifier>()
    );
});




// ===============================
// Worker
// ===============================

builder.Services.AddHostedService<Worker>();



var host =
    builder.Build();


host.Run();

