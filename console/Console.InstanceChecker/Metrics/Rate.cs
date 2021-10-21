using System.Text.Json.Serialization;

namespace Console.InstanceChecker.Metrics
{
    public class Rate
    {
        [JsonPropertyName("fifteenMinuteRate")]
        public double FifteenMinuteRate { get; set; }

        [JsonPropertyName("fiveMinuteRate")] public double FiveMinuteRate { get; set; }

        [JsonPropertyName("meanRate")] public double MeanRate { get; set; }

        [JsonPropertyName("oneMinuteRate")] public double OneMinuteRate { get; set; }
    }
}