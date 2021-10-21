using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Console.InstanceChecker.Metrics
{
    internal class MetricsChecker
    {
        private readonly string _baseUrl;
        private readonly CancellationToken _cancellationToken;
        private readonly TimeSpan _delay;
        private readonly ConcurrentDictionary<string, ServerRate> _metrics;
        private readonly System.Timers.Timer _metricsTimer;

        public readonly Action<ServerRate[]> OnMetricsReceived;

        public MetricsChecker(CancellationToken cancellationToken, string baseUrl, TimeSpan delay,
            Action<ServerRate[]> onMetricsReceived)
        {
            _cancellationToken = cancellationToken;
            _baseUrl = baseUrl;
            _delay = delay;
            OnMetricsReceived = onMetricsReceived;
            _metrics = new ConcurrentDictionary<string, ServerRate>();
            _metricsTimer = new System.Timers.Timer();
            _metricsTimer.Interval = _delay.TotalMilliseconds;
            _metricsTimer.Elapsed += (sender, args) => OnMetricsReceived(_metrics.Values.ToArray());
        }

        public async Task Start()
        {
            using var client = new HttpClient();
            _metricsTimer.Start();
            try
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));

                    var response =
                        await client.GetAsync(new Uri($"{_baseUrl.TrimEnd('/')}/metrics"), _cancellationToken);
                    if (!response.IsSuccessStatusCode)
                    {
                        System.Console.WriteLine($"Unable to get metrics: {response.StatusCode}");
                        continue;
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var metrics = JsonSerializer.Deserialize<Metrics>(json);
                    if (metrics == null)
                        continue;

                    var context = metrics.Contexts.FirstOrDefault();
                    if (context == null)
                        continue;

                    var timer = metrics.Contexts.SelectMany(c => c.Timers).FirstOrDefault();
                    if (timer == null)
                        continue;

                    _metrics[timer.Tags.Server] = new ServerRate
                    {
                        ServerName = context.ContextName,
                        Rate = timer.Rate
                    };
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }

            _metricsTimer.Stop();
        }
    }
}