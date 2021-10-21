using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Console.InstanceChecker.Instance;
using Console.InstanceChecker.Metrics;

namespace Console.InstanceChecker
{
    internal class Program
    {
        private static MetricsChecker _metricsChecker;
        private static Instance.InstanceChecker _instanceChecker;
        private static readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private static Options _options;
        private static int _errors;
        private static int _succeed;

        private static async Task Main(string[] args)
        {
            var sw = new Stopwatch();
            try
            {
                await Parser.Default
                    .ParseArguments<Options>(args)
                    .WithParsedAsync(async options =>
                    {
                        _options = options;
                        _metricsChecker = new MetricsChecker(_cancellationToken.Token, options.Host,
                            TimeSpan.FromSeconds(10),
                            PrintServerRates);
                        _instanceChecker = new Instance.InstanceChecker(_cancellationToken.Token, options.Number,
                            options.Host, TimeSpan.FromMilliseconds(options.DelayMilliseconds), options.MaxThreads,
                            OnError, OnInstanceInfoReceived);

                        System.Console.WriteLine("Started, waiting for events");
                        sw.Restart();
                        await Task.WhenAll(StartInstanceThread(), StartMetricsThread());
                        sw.Stop();
                        _cancellationToken.Cancel();
                    });
            }
            finally
            {
                System.Console.WriteLine(
                    $"Done, elapsed {sw.ElapsedMilliseconds} ms. Succeed: {_succeed}, Errors: {_errors}.");
            }
        }

        private static void OnInstanceInfoReceived(string message, InstanceInfo instanceInfo)
        {
            Interlocked.Increment(ref _succeed);
            if (_options.ShouldPrintInstanceInfo)
                System.Console.WriteLine($"{message}:\t{instanceInfo.Address}");
        }

        private static void OnError(string error)
        {
            Interlocked.Increment(ref _errors);
            System.Console.WriteLine(error);
        }

        private static Task StartMetricsThread()
        {
            return Task.Run(async () =>
            {
                if (!_options.ShouldPrintServerMetrics)
                    return;
                await _metricsChecker.Start();
            }, _cancellationToken.Token);
        }

        private static Task StartInstanceThread()
        {
            return Task.Run(async () => await _instanceChecker.Start(), _cancellationToken.Token);
        }


        private static void PrintServerRates(ServerRate[] rates)
        {
            if (!_options.ShouldPrintServerMetrics)
                return;
            System.Console.WriteLine("------- Server rates --------");

            foreach (var rate in rates)
                System.Console.WriteLine($"Server {rate.ServerName}, {rate.Rate.OneMinuteRate} requests/minute");

            System.Console.WriteLine("------- End of server rates --------");
        }
    }
}