using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
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
            var success = 0;
            var error = 0;
            using (var client = new HttpClient())
            {
                async Task PerformRequestsAndCalculateStat()
                {
                    var result = await DoRequest(options, client);
                    if (result)
                        Interlocked.Increment(ref success);
                    else
                        Interlocked.Increment(ref error);
                }

                async Task PerformBatch(IEnumerable<int> range)
                {
                    foreach (var _ in range)
                    {
                        await PerformRequestsAndCalculateStat();
                    } 
                }
                if (!options.IsParallel)
                {
                    await PerformBatch(Enumerable.Range(0, options.Number));
                }
                else
                {
                    var tasks = new List<Task>();
                    var groups = Enumerable.Range(0, options.Number)
                        .GroupBy(num => num % Environment.ProcessorCount);
                    foreach (var group in groups)
                    {
                        tasks.Add(Task.Run(async() => await PerformBatch(group)));
                    }
                    await Task.WhenAll(tasks);
                }
                
            }
            System.Console.WriteLine($"Succeed: {success}, errored {error}");
        }

        private readonly static object _locker = new object();
        private static async Task<bool> DoRequest(Options options,
            HttpClient client)
        {
            try
            {
                if (options.DelayMilliseconds > 0)
                    await Task.Delay(options.DelayMilliseconds);

                var response = await client.GetAsync(new Uri($"{options.Host.TrimEnd('/')}/instance/current"));
                if (!response.IsSuccessStatusCode)
                {
                    System.Console.WriteLine($"Unable to get instance info: {response.StatusCode}");
                    return false;
                }

                var json = await response.Content.ReadAsStringAsync();
                var info = System.Text.Json.JsonSerializer.Deserialize<InstanceInfo>(json);
                System.Console.WriteLine(info?.Address);
                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"Unable to get instance info: {e.Message}");
                return false;
            }
        }

        private class InstanceInfo
        {
            [JsonPropertyName("address")]
            public string Address { get; set; }
        }
    }
}