using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Nodes;

namespace WordMonitor.Configurator.Services;

public class ConfigService
{
    private readonly string _arquivo;

    public ConfigService(string arquivo)
    {
        _arquivo = arquivo;
    }

    public JsonObject Carregar()
    {
        if (!File.Exists(_arquivo))
        {
            throw new FileNotFoundException(
                "Arquivo appsettings.Local.json não encontrado.",
                _arquivo
            );
        }

        var json = File.ReadAllText(_arquivo);

        var documento = JsonNode.Parse(json);

        if (documento is JsonObject objeto)
            return objeto;

        throw new Exception("Formato inválido de configuração.");
    }

    public void Salvar(JsonObject configuracao)
    {
        var opcoes = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        var json = configuracao.ToJsonString(opcoes);

        File.WriteAllText(_arquivo, json);
    }

    public string ObterValor(JsonObject config, string grupo, string chave)
    {
        return config[grupo]?[chave]?.ToString() ?? "";
    }

    public void AlterarValor(
        JsonObject config,
        string grupo,
        string chave,
        object valor)
    {
        if (config[grupo] is not JsonObject objetoGrupo)
        {
            objetoGrupo = new JsonObject();
            config[grupo] = objetoGrupo;
        }

        objetoGrupo[chave] = JsonValue.Create(valor);
    }
}
