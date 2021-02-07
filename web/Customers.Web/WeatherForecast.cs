using System;
using System.Net;

namespace Customers.Web
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
    
    public class InstanceInfo
    {
        public string Address { get; set; }
    }
}
