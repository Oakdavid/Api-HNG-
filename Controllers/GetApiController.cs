using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_HNG_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetApiController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetApi([FromQuery] string name)
        {
            var clientIp =  "127.0.0.1";
            var location = "New York";
            var temperature = "Hello, Mark!, the temperature is 11 degrees Celcius in New York";

            var Response = new
            {
                clientIp = clientIp,
                location = location,
                greeting = $"Hello,{name}!,the {temperature} is 11 degrees Celcius in New York",
            };

            return Ok(Response);
        }
    }
}
