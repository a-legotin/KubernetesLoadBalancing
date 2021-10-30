using Weather.Web.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Weather.Web.Controllers
{
    [ApiController]
    [Route("instance/current")]
    public class CurrentInstanceController : ControllerBase
    {
        private readonly ILogger<CurrentInstanceController> _logger;

        public CurrentInstanceController(ILogger<CurrentInstanceController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public InstanceInfo Get()
        {
            return new InstanceInfo
            {
                Address = NetworkUtils.GetLocalIPAddress()?.ToString()
            };
        }
    }
}