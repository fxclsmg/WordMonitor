using System.Diagnostics;
using System.ServiceProcess;
using Timer = System.Windows.Forms.Timer;
using System.Reflection;

namespace WordMonitor.Tray;

public class TrayApplicationContext : ApplicationContext
{
    private readonly NotifyIcon _notifyIcon;
    private readonly ContextMenuStrip _menu;
    private readonly Timer _timer;

    private readonly ToolStripMenuItem _statusItem;
    private readonly ToolStripMenuItem _configItem;
    private readonly ToolStripMenuItem _startItem;
    private readonly ToolStripMenuItem _stopItem;
    private readonly ToolStripMenuItem _restartItem;
    private readonly ToolStripMenuItem _exitItem;

    private readonly ServiceManager _service = new();

    public TrayApplicationContext()
    {
        _menu = new ContextMenuStrip();

        _statusItem = new ToolStripMenuItem("Verificando...");
        _statusItem.Enabled = false;

        _configItem = new ToolStripMenuItem(
            "Configurações",
            null,
            AbrirConfigurador_Click);

        _startItem = new ToolStripMenuItem(
            "Iniciar serviço",
            null,
            IniciarServico_Click);

        _stopItem = new ToolStripMenuItem(
            "Parar serviço",
            null,
            PararServico_Click);

        _restartItem = new ToolStripMenuItem(
            "Reiniciar serviço",
            null,
            ReiniciarServico_Click);

        _exitItem = new ToolStripMenuItem(
            "Sair",
            null,
            Sair_Click);

        _menu.Items.Add(_statusItem);
        _menu.Items.Add(new ToolStripSeparator());
        _menu.Items.Add(_configItem);
        _menu.Items.Add(_startItem);
        _menu.Items.Add(_stopItem);
        _menu.Items.Add(_restartItem);
        _menu.Items.Add(new ToolStripSeparator());
        _menu.Items.Add(_exitItem);

        var iconPath = Path.Combine(
            AppContext.BaseDirectory,
            "Resources\\WordMonitor.ico"
        );

        _notifyIcon = new NotifyIcon
        {
            Visible = true,
            Text = "WordMonitor",
            ContextMenuStrip = _menu,
            Icon = new Icon(iconPath)
        };

        _notifyIcon.DoubleClick += AbrirConfigurador_Click;

        _timer = new Timer();
        _timer.Interval = 5000;
        _timer.Tick += AtualizarStatus;

        AtualizarStatus(null, EventArgs.Empty);

        _timer.Start();
    }

    private void AtualizarStatus(object? sender, EventArgs e)
    {
        var iconPath = "";
        try
        {
            using var service = new ServiceController("WordMonitor");

            switch (service.Status)
            {
                case ServiceControllerStatus.Running:

                    _statusItem.Text = "Serviço em execução";

                    iconPath = Path.Combine(
                        AppContext.BaseDirectory,
                        "Resources\\iconGreen.ico"
                    );
                    _notifyIcon.Icon = new Icon (iconPath);


                    break;

                case ServiceControllerStatus.Stopped:

                    _statusItem.Text = "Serviço parado";

                    iconPath = Path.Combine(
                        AppContext.BaseDirectory,
                        "Resources\\iconRed.ico"
                    );
                    _notifyIcon.Icon = new Icon (iconPath);

                    break;

                default:

                    _statusItem.Text = ""+service.Status;

                    iconPath = Path.Combine(
                        AppContext.BaseDirectory,
                        "Resources\\iconYellow.ico"
                    );
                    _notifyIcon.Icon = new Icon (iconPath);

                    break;
            }
        }
        catch
        {
            _statusItem.Text = "❌ Serviço não instalado";
            iconPath = Path.Combine(
                AppContext.BaseDirectory,
                "Resources\\WordMonitor.ico"
            );
            _notifyIcon.Icon = new Icon (iconPath); 

        }
    }

    private void AbrirConfigurador_Click(object? sender, EventArgs e)
    {
        var caminho = Path.Combine(
            AppContext.BaseDirectory,
            "WordMonitor.Configurator.exe");

        if (File.Exists(caminho))
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = caminho,
                UseShellExecute = true
            });
        }
        else
        {
            MessageBox.Show(
                "Configurator não encontrado.",
                "WordMonitor",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void ExecutarServico(string comando)
    {
        var caminho = Path.Combine(
            AppContext.BaseDirectory,
            "WordMonitor.ServiceTool.exe");

        if (!File.Exists(caminho))
        {
            MessageBox.Show(
                "WordMonitor.ServiceTool.exe não encontrado.",
                "WordMonitor",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            return;
        }

        try
        {
            using var processo = Process.Start(new ProcessStartInfo
            {
                FileName = caminho,
                Arguments = comando,
                UseShellExecute = true,
                Verb = "runas"
            });

            processo?.WaitForExit();

            AtualizarStatus(null, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "WordMonitor",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void IniciarServico_Click(object? sender, EventArgs e)
    {
        ExecutarServico("start");
    }


    private void PararServico_Click(object? sender, EventArgs e)
    {
        ExecutarServico("stop");
    }

    private void ReiniciarServico_Click(object? sender, EventArgs e)
    {
        ExecutarServico("restart");
    }
    /*
    private void IniciarServico_Click(object? sender, EventArgs e)
    {
        _service.Iniciar();
        AtualizarStatus(null, EventArgs.Empty);
    }


    private void PararServico_Click(object? sender, EventArgs e)
    {
        _service.Parar();
        AtualizarStatus(null, EventArgs.Empty);
    }

    private void ReiniciarServico_Click(object? sender, EventArgs e)
    {
        _service.Parar();
        _service.Iniciar();
        AtualizarStatus(null, EventArgs.Empty);
    }

    */
    private void Sair_Click(object? sender, EventArgs e)
    {
        _notifyIcon.Visible = false;
        _timer.Stop();
        Application.Exit();
    }
}
