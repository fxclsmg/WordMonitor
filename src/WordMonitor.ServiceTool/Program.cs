using WordMonitor.ServiceTool;

if (args.Length == 0)
{
    return;
}

var service = new ServiceManager();

try
{
    switch (args[0].ToLower())
    {
        case "start":
            service.Iniciar();
            break;

        case "stop":
            service.Parar();
            break;

        case "restart":
            service.Reiniciar();
            break;

        case "install":
            service.Instalar();
            break;

        case "uninstall":
            service.Desinstalar();
            break;

        default:
            Console.WriteLine("Comando inválido.");
            break;
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
