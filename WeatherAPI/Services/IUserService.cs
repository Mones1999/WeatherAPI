namespace WeatherAPI.Services
{
    public interface IUserService
    {
        Task<string> CreateUser(string userName, string passWord);
        Task<string> Login(string userName, string passWord);
       
    }
}
