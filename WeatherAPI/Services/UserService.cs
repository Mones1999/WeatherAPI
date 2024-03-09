namespace WeatherAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IDictionary<string, string> _userData;
        public UserService() 
        { 
            _userData = new Dictionary<string, string>();
        }
        public async Task<string> CreateUser(string userName, string passWord)
        {
            if(_userData.ContainsKey(userName)) return null;

            _userData.Add(userName, passWord);
            return $"The user ({userName}) created successfully";
        }

        public async Task<string> Login(string userName, string passWord)
        {
            if (_userData.ContainsKey(userName))
            {
                if (_userData[userName] == passWord) return $"Login success";
                else return null;
            }
            else 
            {
                return null;
            }
        }

        
    }
}
