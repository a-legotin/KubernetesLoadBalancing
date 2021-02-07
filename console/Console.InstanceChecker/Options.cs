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
    }
}