using System.ServiceProcess;
using System.Diagnostics;

namespace WordMonitor.Tray;

public class ServiceManager
{

    private const string Nome = "WordMonitor";


    public void Iniciar()
    {
        using var s =
            new ServiceController(Nome);


        if(s.Status == ServiceControllerStatus.Stopped)
        {
            s.Start();

            s.WaitForStatus(
                ServiceControllerStatus.Running,
                TimeSpan.FromSeconds(20)
            );
        }
    }



    public void Parar()
    {
        using var s =
            new ServiceController(Nome);


        if(s.CanStop)
        {
            s.Stop();

            s.WaitForStatus(
                ServiceControllerStatus.Stopped,
                TimeSpan.FromSeconds(20)
            );
        }
    }

    
    public void Desinstalar()
    {
        using var service = new ServiceController(Nome);

        if (service.Status != ServiceControllerStatus.Stopped)
        {
            service.Stop();
            service.WaitForStatus(
                ServiceControllerStatus.Stopped,
                TimeSpan.FromSeconds(20));
        }

        Process.Start(new ProcessStartInfo
        {
            FileName = "sc.exe",
            Arguments = $"delete {Nome}",
            UseShellExecute = true,
            Verb = "runas"
        });
    }
}
