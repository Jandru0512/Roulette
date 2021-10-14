using Masiv.Roulette.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Masiv.Roulette.Api
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class RouletteController : ControllerBase
    {
        private readonly ILogger<RouletteController> _logger;
        private readonly IRouletteService _rouletteService;

        public RouletteController(ILogger<RouletteController> logger, IRouletteService rouletteService)
        {
            _logger = logger;
            _rouletteService = rouletteService;
        }

        [HttpPost]
        [Route("createRoulette")]
        public async Task<IActionResult> CreateRoulette()
        {
            _logger.LogInformation("Creating new roulette.");
            int id = await _rouletteService.CreateRoulette();
            if (id != 0)
            {
                _logger.LogInformation($"Roulette {id} created.");
                return Created(string.Empty, id);
            }

            _logger.LogInformation($"Error creating roulette.");
            return BadRequest();
        }
    }
}
