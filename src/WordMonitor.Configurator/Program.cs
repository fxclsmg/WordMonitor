using System;
using System.Windows.Forms;

namespace WordMonitor.Configurator;

internal static class Program
{
    [STAThread]
    static void Main()
    {

        try
        {
            ApplicationConfiguration.Initialize();

            Application.Run(new MainForm());
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.ToString(),
                "Erro:"
            );
        }
    }
}
