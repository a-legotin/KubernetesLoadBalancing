using System;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommandLine;

namespace Console.InstanceChecker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Parser.Default
                .ParseArguments<Options>(args)
                .WithParsedAsync(async options => await CheckInstance(options));
        }

        private static async Task CheckInstance(Options options)
        {
            using (var client = new HttpClient())
            {
                for (var i = 0; i < options.Number; i++)
                {
                    var response = await client.GetAsync(new Uri($"{options.Host.TrimEnd('/')}/instance/current"));
                    if (!response.IsSuccessStatusCode)
                    {
                        System.Console.WriteLine("Unable to get instance info");
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var info = System.Text.Json.JsonSerializer.Deserialize<InstanceInfo>(json);
                    System.Console.WriteLine(info?.Address);
                    await Task.Delay(options.DelayMilliseconds);
                }
            }
        }

        private class InstanceInfo
        {
            [JsonPropertyName("address")]
            public string Address { get; set; }
        }
    }
}