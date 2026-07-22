using System.Diagnostics;

namespace WordMonitor.Configurator.Services;

public class ServiceManager
{
    private const string NomeServico = "WordMonitor";


    public bool Existe()
    {
        var processo = new Process();

        processo.StartInfo.FileName = "sc.exe";
        processo.StartInfo.Arguments = $"query {NomeServico}";
        processo.StartInfo.UseShellExecute = false;
        processo.StartInfo.RedirectStandardOutput = true;
        processo.StartInfo.CreateNoWindow = true;

        processo.Start();

        var resultado =
            processo.StandardOutput.ReadToEnd();

        processo.WaitForExit();


        return resultado.Contains(
            "SERVICE_NAME"
        );
    }



    public void Instalar()
    {
        var caminho =
            Path.Combine(
                AppContext.BaseDirectory,
                "WordMonitor.Worker.exe"
            );


        Executar(
            $"create {NomeServico} binPath= \"{caminho}\" start= auto DisplayName= \"WordMonitor\""
        );


    }


    public void Iniciar()
    {
        Executar(
            $"start {NomeServico}"
        );
    }



    public void Parar()
    {
        Executar(
            $"stop {NomeServico}"
        );
    }



    private void Executar(string argumentos)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "sc.exe",
            Arguments = argumentos,
            UseShellExecute = true,
            Verb = "runas"
        };


        using var processo =
            Process.Start(psi);

        processo?.WaitForExit();
    }
}


/*
using System.Diagnostics;
using System.ServiceProcess;

namespace WordMonitor.Configurator.Services;

public class ServiceManager
{
    private const string NomeServico = "WordMonitor";

    private readonly string _caminhoExe;

    public ServiceManager(string caminhoExe)
    {
        _caminhoExe = caminhoExe;
    }

    public bool Existe()
    {
        return ServiceController
            .GetServices()
            .Any(s => s.ServiceName.Equals(
                NomeServico,
                StringComparison.OrdinalIgnoreCase));
    }

    public bool EstaExecutando()
    {
        if (!Existe())
            return false;

        using var servico = new ServiceController(NomeServico);

        return servico.Status == ServiceControllerStatus.Running;
    }

    public void Instalar()
    {
        if (Existe())
            return;

        ExecutarSc(
            $"create {NomeServico} " +
            $"binPath= \"{_caminhoExe}\" " +
            $"start= auto " +
            $"DisplayName= \"WordMonitor\"");
    }

    public void Desinstalar()
    {
        if (!Existe())
            return;

        if (EstaExecutando())
            Parar();

        ExecutarSc($"delete {NomeServico}");
    }

    public void Iniciar()
    {
        if (!Existe())
            return;

        using var servico = new ServiceController(NomeServico);

        if (servico.Status == ServiceControllerStatus.Running)
            return;

        ExecutarSc($"start {NomeServico}");
    }

    public void Parar()
    {
        if (!Existe())
            return;

        using var servico = new ServiceController(NomeServico);

        if (servico.Status == ServiceControllerStatus.Stopped)
            return;

        ExecutarSc($"stop {NomeServico}");

        servico.WaitForStatus(
            ServiceControllerStatus.Stopped,
            TimeSpan.FromSeconds(30));
    }

    public void Reiniciar()
    {
        if (!Existe())
        {
            Instalar();
            Iniciar();
            return;
        }

        Parar();
        Iniciar();
    }

    private static void ExecutarSc(string argumentos)
    {
        using var processo = Process.Start(new ProcessStartInfo
        {
            FileName = "sc.exe",
            Arguments = argumentos,
            UseShellExecute = false,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden
        });

        processo?.WaitForExit();

        if (processo == null)
            throw new Exception("Não foi possível executar sc.exe.");

        if (processo.ExitCode != 0)
            throw new Exception($"sc.exe retornou erro {processo.ExitCode}.");
    }
}
*/

