using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WeatherAPI.Controllers;
using WeatherAPI.Data;
using WeatherAPI.Services;

namespace WeatherAPI.Tests
{
    public class WeatherControllerTests
    {
        private readonly WeatherController _weatherController;
        private readonly Mock<IWeatherService> _weatherServiceMock;
        private readonly Mock<ILogger<WeatherController>> _loggerMock;

        public WeatherControllerTests()
        {
            _weatherServiceMock = new Mock<IWeatherService>();
            _loggerMock = new Mock<ILogger<WeatherController>>();
            _weatherController = new WeatherController(_weatherServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetWeather_Success()
        {
            // Arrange
            string city = "Irbid";
            var weatherData = new WeatherData();
            _weatherServiceMock.Setup(service => service.GetWeatherData(city)).ReturnsAsync(weatherData);

            // Act
            var result = await _weatherController.GetWeather(city) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(weatherData, result.Value);
        }

        [Fact]
        public async Task GetWeather_Exception()
        {
            // Arrange
            string city = "Irbid";
            _weatherServiceMock.Setup(service => service.GetWeatherData(city)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _weatherController.GetWeather(city) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal("Internal server error", result.Value);
        }

        [Fact]
        public async Task GetWeather_NullResult()
        {
            // Arrange
            string city = "CityNotFound";
            _weatherServiceMock.Setup(service => service.GetWeatherData(city)).ReturnsAsync((WeatherData)null);

            // Act
            var result = await _weatherController.GetWeather(city) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("An error occurred while processing your request", result.Value);
        }

        
    }
}
