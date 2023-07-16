using Weather.Web.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Weather.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/current")]
    public class InstanceController : ControllerBase
    {
        private readonly ILogger<InstanceController> _logger;

        public InstanceController(ILogger<InstanceController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public InstanceInfo Get() => new InstanceInfo
        {
            Address = NetworkUtils.GetLocalIPAddress()?.ToString() ?? "unavailable"
        };
    }
}