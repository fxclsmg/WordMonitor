using WordMonitor.Notifications;
using WordMonitor.Services;
using WordMonitor.Worker;
using WordMonitor.Models;
using WordMonitor.Configuration;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile("appsettings.Local.json", true, true);

// ===============================
// Serviço windows
// ===============================

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "WordMonitor";
});

// ===============================
// Config
// ===============================

builder.Services.Configure<MonitorConfig>(
    builder.Configuration.GetSection("Monitor"));

builder.Services.Configure<ValidadeConfig>(
    builder.Configuration.GetSection("Validade"));

builder.Services.Configure<EmailConfig>(
    builder.Configuration.GetSection("Email"));

builder.Services.Configure<NotificarConfig>(
    builder.Configuration.GetSection("Notificar"));

builder.Services.Configure<MensagensConfig>(
    builder.Configuration.GetSection("Mensagens"));

builder.Services.Configure<VerificacaoConfig>(
    builder.Configuration.GetSection("Verificacao"));

// ===============================
// Injeção de dependências
// ===============================

builder.Services.AddSingleton<DocumentScanner>();
builder.Services.AddSingleton<ParserService>();
builder.Services.AddSingleton<ValidityChecker>();
builder.Services.AddSingleton<DocumentMonitorService>();
builder.Services.AddSingleton<ExpirationService>();

builder.Services.AddSingleton<LogNotifier>();
builder.Services.AddSingleton<EmailNotifier>();
builder.Services.AddSingleton<INotifier, CompositeNotifier>();
builder.Services.AddSingleton<NotificationBuilder>();

builder.Services.AddHostedService<Worker>();

// ===============================
// Executa o Worker
// ===============================

var host =
    builder.Build();

host.Run();

