using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Console.InstanceChecker.Metrics
{
    public class Counter
    {
        [JsonPropertyName("count")] public int Count { get; set; }

        [JsonPropertyName("items")] public List<object> Items { get; set; }

        [JsonPropertyName("name")] public string Name { get; set; }

        [JsonPropertyName("tags")] public Tags Tags { get; set; }

        [JsonPropertyName("unit")] public string Unit { get; set; }
    }
}