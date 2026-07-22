using System.Diagnostics;
using System.ServiceProcess;


namespace WordMonitor.ServiceTool;

public class ServiceManager
{
    private const string NomeServico = "WordMonitor";

    public void Iniciar()
    {
        using var service = new ServiceController(NomeServico);

        service.Refresh();

        if (service.Status == ServiceControllerStatus.Running)
            return;

        service.Start();

        service.WaitForStatus(
            ServiceControllerStatus.Running,
            TimeSpan.FromSeconds(20));
    }

    public void Parar()
    {
        using var service = new ServiceController(NomeServico);

        service.Refresh();

        if (service.Status == ServiceControllerStatus.Stopped)
            return;

        if (!service.CanStop)
            throw new InvalidOperationException(
                "O serviço não pode ser parado.");

        service.Stop();

        service.WaitForStatus(
            ServiceControllerStatus.Stopped,
            TimeSpan.FromSeconds(20));
    }

    public void Reiniciar()
    {
        using var service = new ServiceController(NomeServico);

        service.Refresh();

        if (service.Status != ServiceControllerStatus.Stopped)
        {
            if (!service.CanStop)
                throw new InvalidOperationException(
                    "O serviço não pode ser parado.");

            service.Stop();

            service.WaitForStatus(
                ServiceControllerStatus.Stopped,
                TimeSpan.FromSeconds(20));
        }

        service.Start();

        service.WaitForStatus(
            ServiceControllerStatus.Running,
            TimeSpan.FromSeconds(20));
    }

    public void Instalar()
    {
        var exe = Path.Combine(
            AppContext.BaseDirectory,
            "WordMonitor.Worker.exe");

        if (!File.Exists(exe))
            throw new FileNotFoundException(
                "WordMonitor.Worker.exe não encontrado.");

        ExecutarSC(
            $"create {NomeServico} binPath= \"{exe}\" start= auto");

        ExecutarSC(
            $"description {NomeServico} \"Monitor de documentos Word\"");
    }

    public void Desinstalar()
    {
        try
        {
            Parar();
        }
        catch
        {
            // Ignora caso já esteja parado
        }

        ExecutarSC($"delete {NomeServico}");
    }

    private static void ExecutarSC(string argumentos)
    {
        using var processo = Process.Start(new ProcessStartInfo
        {
            FileName = "sc.exe",
            Arguments = argumentos,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        });

        processo!.WaitForExit();

        if (processo.ExitCode != 0)
        {
            var erro = processo.StandardError.ReadToEnd();

            if (string.IsNullOrWhiteSpace(erro))
                erro = processo.StandardOutput.ReadToEnd();

            throw new Exception(erro);
        }
    }
}
