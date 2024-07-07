using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace API_HNG_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetApiController : ControllerBase
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public GetApiController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetApi([FromQuery] string name)
        {
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            double temperature = 0;
            string city = "" ;

            if (!string.IsNullOrEmpty(clientIp))
            {
                var httpClient = _httpClientFactory.CreateClient();
                var clientIpData = await httpClient.GetStringAsync($"http://ip-api.com/json/{clientIp}");
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var details = JsonSerializer.Deserialize<JsonElement>(clientIpData, options);

                if (details.TryGetProperty("city", out JsonElement cityElement))
                {
                    city = cityElement.GetString();
                }

                if (!string.IsNullOrEmpty(city))
                {
                    var apiKey = "d45c77922473f77e95e106788ee2bb9f";
                    var weatherResponse = await httpClient.GetStringAsync($"https://api.weatherapi.com/v1/current.json?key={apiKey}&q={city}");
                    var weatherData = JsonSerializer.Deserialize<JsonElement>(weatherResponse, options);

                    if (weatherData.TryGetProperty("current", out JsonElement currentElement) &&
                        currentElement.TryGetProperty("temp_c", out JsonElement tempElement))
                    {
                        temperature = tempElement.GetDouble();
                    }
                }
            }

            var response = new
            {
                clientIp = clientIp,
                location = city,
                greeting = $"Hello, {name}! The temperature is {temperature} degrees Celsius in {city}.",
            };

            return Ok(response);
        }

    }
}
