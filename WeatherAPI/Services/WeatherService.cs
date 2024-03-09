using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using WeatherAPI.Data;

namespace WeatherAPI.Services
{
    public class WeatherService : IWeatherService
    {
        private const string key = "725b54314316f32822b8508d40fe51c2";
        private const string url = "https://api.openweathermap.org/data/2.5/weather";
        private readonly ILogger<WeatherService> _logger;
        private readonly HttpClient _httpClient;
        public WeatherService(ILogger<WeatherService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }
        public async Task<WeatherData> GetWeatherData(string city)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{url}?q={city}&appid={key}&units=metric");
                if (response.IsSuccessStatusCode)
                {
                    var weatherData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<WeatherData>(weatherData);
                }
                else
                {
                    _logger.LogError($"Failed to get weather data for {city}. Status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"An error occurred while fetching weather data for {city}: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogError($"An error occurred while parsing weather data for {city}: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred while fetching weather data for {city}: {ex.Message}");
                return null;
            }
        }
    }
}
