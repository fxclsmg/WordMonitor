using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;

namespace WordMonitor.Services;

public class WordReader
{
    public string LerTexto(string caminho)
    {
        StringBuilder texto = new();

        using var documento = WordprocessingDocument.Open(caminho, false);

        var body = documento.MainDocumentPart?.Document?.Body;

        if (body == null)
            return string.Empty;

        foreach (var paragrafo in body.Elements<Paragraph>())
        {
            foreach (var t in paragrafo.Descendants<Text>())
            {
                texto.Append(t.Text);
            }

            texto.AppendLine();
        }

        return texto.ToString();
    }
}
