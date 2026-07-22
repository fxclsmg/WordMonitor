using System;
using System.Windows.Forms;

namespace WordMonitor.Tray;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        Application.Run(
            new TrayApplicationContext()
        );
    }
}
