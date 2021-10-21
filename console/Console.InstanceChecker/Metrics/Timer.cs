using System.Text.Json.Serialization;

namespace Console.InstanceChecker.Metrics
{
    public class Timer
    {
        [JsonPropertyName("activeSessions")] public int ActiveSessions { get; set; }

        [JsonPropertyName("count")] public int Count { get; set; }

        [JsonPropertyName("durationUnit")] public string DurationUnit { get; set; }

        [JsonPropertyName("histogram")] public Histogram Histogram { get; set; }

        [JsonPropertyName("rate")] public Rate Rate { get; set; }

        [JsonPropertyName("rateUnit")] public string RateUnit { get; set; }

        [JsonPropertyName("name")] public string Name { get; set; }

        [JsonPropertyName("tags")] public Tags Tags { get; set; }

        [JsonPropertyName("unit")] public string Unit { get; set; }
    }
}