using System;
using System.Windows.Forms;
using WordMonitor.Configurator.Forms;

namespace WordMonitor.Configurator;

internal static class Program
{
    [STAThread]
    static void Main()
    {

        try
        {
            ApplicationConfiguration.Initialize();

            //Application.Run(new MainForm());
            Application.Run(new FormMain());
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.ToString(),
                "Erro"
            );
        }
    }
}
