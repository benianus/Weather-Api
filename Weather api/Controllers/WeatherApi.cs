using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private ThirdPartyApiLayer ThirdPartyApi { get; set; }
        private readonly IMemoryCache _cache;
        public WeatherApi(ThirdPartyApiLayer thirdPartyApiLayer, IMemoryCache cache)
        {
            ThirdPartyApi = thirdPartyApiLayer;
            _cache = cache;
        }

        [HttpGet("Json/{location}", Name = "GetTodayTempertaureJson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTodayTemperatureJson(string location)
        {
            // if the data already exists in the cache the value will return directly
            // if not it will execute the entry code and get the data from source and save it in the cache
            var data = await _cache.GetOrCreateAsync("weatherApi", async entry => {

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                var weather = await ThirdPartyApi.GetTodayTemperatureByLocationJson(location);

                if (weather == null)
                {
                    return null;
                }

                return weather;
            });

            if (data == null)
            {
                return NotFound("Data Not Found");
            }

            return Ok(data);
        }
    }
}

//"weatherApi", async (entry) =>
//{
//    Weather? weatherData = await ThirdPartyApi.GetTodayTemperatureByLocationJson(location);

//    if (weatherData == null)
//    {
//        return NotFound("Weather infos not foud");
//    }

//    return weatherData;
//}