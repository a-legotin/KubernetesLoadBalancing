using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Console.InstanceChecker.Instance
{
    public class InstanceInfo
    {
        [JsonPropertyName("address")] public string Address { get; set; }
    }

    public class InstanceChecker
    {
        private readonly string _baseUrl;

        private readonly CancellationToken _cancellationToken;
        private readonly TimeSpan _delay;
        private readonly int _maxThreads;
        private readonly Action<string> _onError;
        private readonly Action<string, InstanceInfo> _onInstanceInfoReceived;
        private readonly ConcurrentStack<int> _requestsStack;

        public InstanceChecker(CancellationToken cancellationToken, int maxRequests, string baseUrl, TimeSpan delay,
            int maxThreads, Action<string> onError, Action<string, InstanceInfo> onInstanceInfoReceived)
        {
            _cancellationToken = cancellationToken;
            _baseUrl = baseUrl;
            _delay = delay;
            _maxThreads = maxThreads;
            _onError = onError;
            _onInstanceInfoReceived = onInstanceInfoReceived;
            _requestsStack = new ConcurrentStack<int>();
            foreach (var num in Enumerable.Range(0, maxRequests)) _requestsStack.Push(num);
        }

        public async Task Start()
        {
            var tasks = Enumerable.Range(1, _maxThreads).Select(_ => Task.Run(PerformRequest, _cancellationToken));
            await Task.WhenAll(tasks);
        }

        private async Task PerformRequest()
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(1);
            while (!_cancellationToken.IsCancellationRequested && !_requestsStack.IsEmpty)
                try
                {
                    if (_delay.Milliseconds > 0)
                        await Task.Delay(_delay.Milliseconds, _cancellationToken);

                    if (!_requestsStack.TryPop(out var _))
                        continue;
                    var response = await client.GetAsync(new Uri($"{_baseUrl.TrimEnd('/')}/instance/current"),
                        _cancellationToken);
                    if (!response.IsSuccessStatusCode)
                    {
                        _onError(response.StatusCode.ToString());
                        continue;
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var info = JsonSerializer.Deserialize<InstanceInfo>(json);
                    _onInstanceInfoReceived($"[{Thread.CurrentThread.ManagedThreadId}]", info);
                }
                catch (Exception e)
                {
                    _onError(e.Message);
                }
        }
    }
}