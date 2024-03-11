using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using WeatherAPI.Controllers;
using WeatherAPI.Services;

namespace WeatherAPI.Tests
{
    public class UserControllerTests
    {
        private readonly UserController _userController;
        private readonly Mock<IUserService> _userServiceMock;

        public UserControllerTests()
        {
            IConfiguration configuration = new ConfigurationBuilder().Build();
            _userServiceMock = new Mock<IUserService>();
            _userController = new UserController(_userServiceMock.Object, configuration);
        }

        [Fact]
        public async Task RegisterUser_Success()
        {
            // Arrange
            string userName = "Mones123";
            string password = "testPassword";
            string returnedValue = $"The user ({userName}) created successfully";
            _userServiceMock.Setup(service => service.CreateUser(userName, password)).ReturnsAsync(returnedValue);

            // Act
            var result = await _userController.RegisterUser(userName, password) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal($"The user ({userName}) created successfully", result.Value);
        }

        [Fact]
        public async Task RegisterUser_Failure()
        {
            // Arrange
            string userName = "Mones123";
            string password = "testPassword";

            _userServiceMock.Setup(service => service.CreateUser(userName, password)).ReturnsAsync((string)null);

            // Act
            var result = await _userController.RegisterUser(userName, password) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Register failed", result.Value);
        }

        [Fact]
        public async Task Login_Success()
        {
            // Arrange
            string userName = "Mones123";
            string password = "testPassword";
            string res = @"{
                    ""result"": ""Login success"",
                    ""token"": ""eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyTmFtZSI6Ik1vbmVzIiwiZXhwIjoxNzA5OTg3OTA3fQ.92sOWLIRl5u5bFGdLNPzKX52mAlAYA3oVn5_gRf97Qc""
                    }";
            _userServiceMock.Setup(service => service.Login(userName, password)).ReturnsAsync(res);

            // Act
            var result = await _userController.Login(userName, password) as OkObjectResult;

            // Assert
            
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(true, result.Value.ToString().Contains("token"));

        }

        [Fact]
        public async Task Login_Failure()
        {
            // Arrange
            string userName = "Mones";
            string password = "invalidPassword";

            _userServiceMock.Setup(service => service.Login(userName, password)).ReturnsAsync((string)null);

            // Act
            var result = await _userController.Login(userName, password) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Login failed", result.Value);
        }
    }
}
