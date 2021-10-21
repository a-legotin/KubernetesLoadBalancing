using System.Text.Json.Serialization;

namespace Console.InstanceChecker.Metrics
{
    public class Histogram
    {
        [JsonPropertyName("lastValue")] public double LastValue { get; set; }

        [JsonPropertyName("max")] public double Max { get; set; }

        [JsonPropertyName("mean")] public double Mean { get; set; }

        [JsonPropertyName("median")] public double Median { get; set; }

        [JsonPropertyName("min")] public double Min { get; set; }

        [JsonPropertyName("percentile75")] public double Percentile75 { get; set; }

        [JsonPropertyName("percentile95")] public double Percentile95 { get; set; }

        [JsonPropertyName("percentile98")] public double Percentile98 { get; set; }

        [JsonPropertyName("percentile99")] public double Percentile99 { get; set; }

        [JsonPropertyName("percentile999")] public double Percentile999 { get; set; }

        [JsonPropertyName("sampleSize")] public int SampleSize { get; set; }

        [JsonPropertyName("stdDev")] public double StdDev { get; set; }

        [JsonPropertyName("sum")] public double Sum { get; set; }
    }
}