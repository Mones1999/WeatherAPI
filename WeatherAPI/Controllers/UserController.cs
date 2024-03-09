using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WeatherAPI.Services;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(string userName, string passWord)
        {
            var result =  await _userService.CreateUser(userName, passWord);

            if(result != null) 
            { 
                return Ok(result); 
            }
            else 
            { 
                return BadRequest("Register failed");  
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string userName, string passWord)
        {
            var result = await _userService.Login(userName, passWord);
            
            if (result == null)
            {
                return BadRequest("Login failed");
            }
            
            var token = GenerateJwtToken(userName);
            return Ok(new { Result = result, Token = token });
            
        }

        

        private string GenerateJwtToken(string userName)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var signCredintails = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claimes = new List<Claim>
            {
                new Claim ("userName",userName),
                
            };
            var tokenOptions = new JwtSecurityToken(
                claims: claimes,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: signCredintails
                ); ;
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;

        }
    }
}
