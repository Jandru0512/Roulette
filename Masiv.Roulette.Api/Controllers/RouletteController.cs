using Masiv.Roulette.Common;
using Masiv.Roulette.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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

        [HttpPut]
        [Route("closeRoulette")]
        public async Task<IActionResult> CloseRoulette([FromBody] RouletteDto roulette)
        {
            _logger.LogInformation($"Closing roulette {roulette.Id}.");
            List<BetDto> bets = await _rouletteService.CloseRoulette(roulette.Id);
            _logger.LogInformation($"Roulette {roulette.Id} closed.");
            return Created(string.Empty, bets);
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

        [HttpPut]
        [Route("openRoulette")]
        public async Task<IActionResult> OpenRoulette([FromBody] RouletteDto roulette)
        {
            _logger.LogInformation($"Opening roulette {roulette.Id}.");
            if (roulette.Id != 0)
            {
                bool status = await _rouletteService.OpenRoulette(roulette.Id);
                if (status)
                {
                    _logger.LogInformation($"Roulette {roulette.Id} opened.");
                    return Created(string.Empty, status);
                }
            }
            _logger.LogInformation($"Error opening roulette.");
            return BadRequest();
        }
    }
}
