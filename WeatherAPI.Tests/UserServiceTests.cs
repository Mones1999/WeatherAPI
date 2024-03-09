using WeatherAPI.Services;

namespace WeatherAPI.Tests
{
    public class UserServiceTests
    {

        [Fact]
        public async Task CreateUser_CreateNewUser_Success()
        {
            // Arrange
            var userService = new UserService();
            var userName = "Mones123";
            var password = "TestPassword";

            // Act
            var result = await userService.CreateUser(userName, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal($"The user ({userName}) created successfully", result);
        }
        [Fact]
        public async Task CreateUser_CreateUserAlreadyFound_ReturnNull()
        {
            // Arrange
            var userService = new UserService();
            var userName = "Mones123";
            var password = "TestPassword";
            await userService.CreateUser(userName, password);

            // Act
            var result = await userService.CreateUser(userName, password);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Login_LoginToUserAccount_Success()
        {
            // Arrange
            var userService = new UserService();
            var userName = "Mones123";
            var password = "testPassword";

            await userService.CreateUser(userName, password);

            // Act
            var result = await userService.Login(userName, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Login success", result);
        }

        [Fact]
        public async Task Login_LoginToNonExistentUser_UserNotFound()
        {
            // Arrange
            var userService = new UserService();
            var userName = "Mones";
            var password = "testPassword";

            // Act
            var result = await userService.Login(userName, password);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Login_Failure_IncorrectPassword()
        {
            // Arrange
            var userService = new UserService();
            var userName = "Mones123";
            var correctPassword = "correctPassword";
            var incorrectPassword = "incorrectPassword";

            await userService.CreateUser(userName, correctPassword);

            // Act
            var result = await userService.Login(userName, incorrectPassword);

            // Assert
            Assert.Null(result);
        }

    }
}
