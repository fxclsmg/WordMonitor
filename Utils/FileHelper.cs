namespace WordMonitor.Utils;

public static class FileHelper
{
    public static async Task<bool> AguardarArquivoDisponivel(
        string caminho,
        int tentativas = 10,
        int intervaloMs = 500)
    {
        for (int i = 0; i < tentativas; i++)
        {
            try
            {
                using FileStream stream = new(
                    caminho,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.None);

                return true;
            }
            catch (IOException)
            {
                await Task.Delay(intervaloMs);
            }
            catch (UnauthorizedAccessException)
            {
                await Task.Delay(intervaloMs);
            }
        }

        return false;
    }
}
