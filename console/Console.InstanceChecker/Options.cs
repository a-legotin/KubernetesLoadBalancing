using CommandLine;

namespace Console.InstanceChecker
{
    public class Options
    {
        [Option('n', "number", Required = false, HelpText = "Number of retries")]
        public int Number { get; set; }

        [Option('h', "host", Required = false, HelpText = "Host to check")]
        public string Host { get; set; }

        [Option('d', "delay", Required = false, HelpText = "Delay in milliseconds")]
        public int DelayMilliseconds { get; set; }

        [Option('t', "threads", Required = false, HelpText = "Max threads")]
        public int MaxThreads { get; set; }

        [Option('i', "print-instance", Required = false, HelpText = "Print instance info")]
        public bool ShouldPrintInstanceInfo { get; set; }

        [Option('m', "print-metrics", Required = false, HelpText = "Print server metrics")]
        public bool ShouldPrintServerMetrics { get; set; }
    }
}