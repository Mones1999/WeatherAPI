using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using WeatherAPI.Data;
using WeatherAPI.Services;

namespace WeatherAPI.Tests
{
    public class WeatherServiceTests
    {
        private readonly IWeatherService _weatherService;
        private readonly Mock<ILogger<WeatherService>> _loggerMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;

        public WeatherServiceTests()
        {
            _loggerMock = new Mock<ILogger<WeatherService>>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _weatherService = new WeatherService(_loggerMock.Object, _httpClient);
        }

        [Fact]
        public async Task GetWeatherData_SendInvalidCity_ReturnNull()
        {
            // Arrange
            var city = "CityNotFound";
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _weatherService.GetWeatherData(city);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetWeatherData_SendValidCity_ReturnSuccessfully()
        {
            // Arrange
            var city = "Irbid";
            var expectedWeatherData = new WeatherData
            {
                Coord = new Coord { Lon = 35.8333, Lat = 32.5 },
                Weather = new Weather[] { new Weather { Id = 803, Main = "Clouds", Description = "broken clouds", Icon = "04d" } },
                Base = "stations",
                Main = new Main
                {
                    Temp = 11.31,
                    Feels_like = 10.32,
                    Temp_min = 11.31,
                    Temp_max = 11.31,
                    Pressure = 1017,
                    Humidity = 70,
                    sea_level = 1017,
                    grnd_level = 934
                },
                Visibility = 10000,
                Wind = new Wind { Speed = 5.32, Deg = 252, Gust = 6.6 },
                Clouds = new Clouds { All = 54 },
                Dt = 1709966641,
                Sys = new Sys { Country = "JO", Sunrise = 1709956468, Sunset = 1709998804 },
                Timezone = 10800,
                Id = 248944,
                Name = "Irbid Governorate",
                Cod = 200
            };

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            httpResponseMessage.Content = new StringContent(@"{
            ""coord"": {
            ""lon"": 35.8333,
            ""lat"": 32.5
            },
            ""weather"": [
            {
            ""id"": 803,
            ""main"": ""Clouds"",
            ""description"": ""broken clouds"",
            ""icon"": ""04d""
            }
            ],
            ""base"": ""stations"",
            ""main"": {
                ""temp"": 11.31,
                ""feels_like"": 10.32,
                ""temp_min"": 11.31,
                ""temp_max"": 11.31,
                ""pressure"": 1017,
                ""humidity"": 70,
                ""sea_level"": 1017,
                ""grnd_level"": 934
            },
            ""visibility"": 10000,
            ""wind"": {
                ""speed"": 5.32,
                ""deg"": 252,
                ""gust"": 6.6
            },
            ""clouds"": {
                ""all"": 54
            },
            ""dt"": 1709966641,
            ""sys"": {
                ""country"": ""JO"",
                ""sunrise"": 1709956468,
                ""sunset"": 1709998804
            },
            ""timezone"": 10800,
            ""id"": 248944,
            ""name"": ""Irbid Governorate"",
            ""cod"": 200
        }");

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _weatherService.GetWeatherData(city);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedWeatherData.ToString(), result.ToString());
        }
    }
}