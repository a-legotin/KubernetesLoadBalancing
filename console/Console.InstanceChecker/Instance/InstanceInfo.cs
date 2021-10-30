using System.Text.Json.Serialization;

namespace Console.InstanceChecker.Instance
{
    public class InstanceInfo
    {
        [JsonPropertyName("address")] public string Address { get; set; }
    }
}