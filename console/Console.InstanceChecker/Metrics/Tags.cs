using System.Text.Json.Serialization;

namespace Console.InstanceChecker.Metrics
{
    public class Tags
    {
        [JsonPropertyName("server")] public string Server { get; set; }

        [JsonPropertyName("app")] public string App { get; set; }

        [JsonPropertyName("env")] public string Env { get; set; }
    }
}