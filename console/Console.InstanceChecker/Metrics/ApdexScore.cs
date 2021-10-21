using System.Text.Json.Serialization;

namespace Console.InstanceChecker.Metrics
{
    public class ApdexScore
    {
        [JsonPropertyName("frustrating")] public int Frustrating { get; set; }

        [JsonPropertyName("sampleSize")] public int SampleSize { get; set; }

        [JsonPropertyName("satisfied")] public int Satisfied { get; set; }

        [JsonPropertyName("score")] public int Score { get; set; }

        [JsonPropertyName("tolerating")] public int Tolerating { get; set; }

        [JsonPropertyName("name")] public string Name { get; set; }

        [JsonPropertyName("tags")] public Tags Tags { get; set; }
    }
}