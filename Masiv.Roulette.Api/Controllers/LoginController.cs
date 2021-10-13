using Masiv.Roulette.Common;
using Masiv.Roulette.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Masiv.Roulette.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILoginService _loginService;

        public LoginController(IConfiguration configuration, ILoginService loginService)
        {
            _configuration = configuration;
            _loginService = loginService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            TokenDto token = new TokenDto
            {
                Audience = _configuration["Tokens:Audience"],
                Issuer = _configuration["Tokens:Issuer"],
                Key = _configuration["Tokens:Key"]
            };
            AuthenticationDto authentication = await _loginService.Login(login, token);
            return Created(string.Empty, authentication);
        }
    }
}
