using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Console.InstanceChecker.Metrics
{
    public class Context
    {
        [JsonPropertyName("apdexScores")] public List<ApdexScore> ApdexScores { get; set; }

        [JsonPropertyName("counters")] public List<Counter> Counters { get; set; }

        [JsonPropertyName("context")] public string ContextName { get; set; }

        [JsonPropertyName("gauges")] public List<object> Gauges { get; set; }

        [JsonPropertyName("histograms")] public List<object> Histograms { get; set; }

        [JsonPropertyName("bucketHistograms")] public List<object> BucketHistograms { get; set; }

        [JsonPropertyName("meters")] public List<object> Meters { get; set; }

        [JsonPropertyName("timers")] public List<Timer> Timers { get; set; }

        [JsonPropertyName("bucketTimers")] public List<object> BucketTimers { get; set; }
    }
}