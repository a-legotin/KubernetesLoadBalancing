using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Customers.Web.Controllers
{
    [ApiController]
    [Route("instance/current")]
    public class CurrentInstanceController : ControllerBase
    {
        private readonly ILogger<CurrentInstanceController> _logger;

        public CurrentInstanceController(ILogger<CurrentInstanceController> logger) => _logger = logger;

        [HttpGet]
        public InstanceInfo Get()
        {
            IPAddress LocalIPAddress()
            {
                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    return null;
                }

                var host = Dns.GetHostEntry(Dns.GetHostName());

                return host
                    .AddressList
                    .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            }

            return new InstanceInfo
            {
                Address = LocalIPAddress()?.ToString()
            };
        }
    }
}