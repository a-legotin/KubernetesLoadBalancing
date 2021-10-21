using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Console.InstanceChecker.Metrics
{
    public class Metrics
    {
        [JsonPropertyName("contexts")] public List<Context> Contexts { get; set; }

        [JsonPropertyName("timestamp")] public DateTime Timestamp { get; set; }
    }
}