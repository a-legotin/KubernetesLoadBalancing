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
        private ConcurrentBag<string> _latestMetrics = new ConcurrentBag<string>();
        private readonly int _minMetricsToClean = 50;
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
            _metricsTimer.Elapsed += (sender, args) =>
            {
                if(_cancellationToken.IsCancellationRequested)
                    _metricsTimer.Stop();
                var uniqueMetrics = _latestMetrics.GroupBy(s => s)
                    .SelectMany(s => s)
                    .ToArray();

                if (_latestMetrics.Count > _minMetricsToClean)
                {
                    foreach (var metric in _metrics.Keys.ToArray())
                    {
                        if (!uniqueMetrics.Contains(metric))
                            _metrics.TryRemove(metric, out _);
                    }
                    _latestMetrics.Clear();
                }
                
                OnMetricsReceived(_metrics.Values.ToArray());
            };
        }

        public async Task Start()
        {
            using var client = new HttpClient();
            _metricsTimer.Start();
            
                while (!_cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                    await Task.Delay(TimeSpan.FromMilliseconds(250), _cancellationToken);

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
                        ServerName = timer.Tags.Server,
                        Rate = timer.Rate
                    };
                    _latestMetrics.Add(timer.Tags.Server);
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine(e);
                    }
                }
           

            _metricsTimer.Stop();
        }
    }
}