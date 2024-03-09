
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Authentication;
using WeatherAPI.Services;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherController> _logger;
        
        public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
           
        }

        [HttpGet("{city}")]
        [JwtAuthorizationFilter]
        public async Task<IActionResult> GetWeather(string city)
        {
            try
            {
                
                var result = await _weatherService.GetWeatherData(city);

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    _logger.LogWarning($"Failed to retrieve weather data for {city}");
                    return BadRequest("An error occurred while processing your request");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while processing the request for {city}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
