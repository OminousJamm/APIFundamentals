using APIFundamentals.Dtos;
using APIFundamentals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIFundamentals.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly  IConfiguration _configuration;
        private readonly ILogginService _logginService;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration, ILogginService logginService)
        {
            _logger = logger;
            _configuration = configuration;
            _logginService = logginService;
            
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody]LoginDtos.Login login) {

            if (login.User is null || login.Password is null) {
                return BadRequest($"User or passwword not defined");
            }

            var claims = new[] { new Claim(ClaimTypes.Name, login.User) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWTSecret")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: creds);
            _logginService.Log("Success saved log in post login");
            return Ok(new { login.User, token = new JwtSecurityTokenHandler().WriteToken(token) });

        }

        [Authorize]
        [Authorize(Policy = "AdminOnly")]
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
