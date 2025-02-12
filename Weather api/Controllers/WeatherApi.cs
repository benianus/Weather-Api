using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThirdPartyApi;
using ThirdPartyApi.Models;

namespace Weather_API.Controllers
{
    [Route("api/todayTemperture")]
    [ApiController]
    public class WeatherApi : ControllerBase
    {
        // get data from appsettings.json usin IConfiguration interface
        // Inject the interface to the Weather api class
        private readonly IConfiguration configuration;
        private ThirdPartyApiLayer ThirdPartyApi { get; set; }
        private readonly string? apiKey;
        public WeatherApi(IConfiguration Configuration)
        {
            ThirdPartyApi = new ThirdPartyApiLayer();
            configuration = Configuration;
            apiKey = configuration["ApiKey"];
        }

        [HttpGet("Json/{location}", Name = "GetTodayTempertaureJson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Weather>> GetTodayTemperatureJson(string location)
        {
            var weatherData = await ThirdPartyApi.GetTodayTemperatureByLocationJson(location, apiKey);

            if (weatherData == null)
            {
                return NotFound("Weather infos not foud");
            }

            return Ok(weatherData);
        }
    }
}
