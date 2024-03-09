using WeatherAPI.Data;

namespace WeatherAPI.Services
{
    public interface IWeatherService
    {
        Task<WeatherData> GetWeatherData(string city);
    }
}
