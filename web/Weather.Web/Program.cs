using App.Metrics.AspNetCore;
using Weather.Web.Classes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Weather.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureMetricsWithDefaults(
                    builder =>
                    {
                        builder.Configuration.Configure(options =>
                            options.DefaultContextLabel = NetworkUtils.GetLocalIPAddress().ToString());
                    })
                .UseMetrics()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}