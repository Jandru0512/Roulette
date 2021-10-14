using Masiv.Roulette.Common;
using Masiv.Roulette.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Masiv.Roulette.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginService _loginService;

        public LoginController(IConfiguration configuration, ILogger<LoginController> logger, ILoginService loginService)
        {
            _configuration = configuration;
            _logger = logger;
            _loginService = loginService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            _logger.LogInformation($"Logging user {login.Username}.");
            TokenDto token = new()
            {
                Audience = _configuration["Tokens:Audience"],
                Issuer = _configuration["Tokens:Issuer"],
                Key = _configuration["Tokens:Key"]
            };
            AuthenticationDto authentication = await _loginService.Login(login, token);
            if (authentication != null)
            {
                _logger.LogInformation($"User {login.Username} logged.");
                return Created(string.Empty, authentication);
            }

            _logger.LogInformation($"Error logging {login.Username}.");
            return BadRequest();
        }
    }
}
