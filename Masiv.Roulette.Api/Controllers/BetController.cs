using Masiv.Roulette.Common;
using Masiv.Roulette.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Masiv.Roulette.Api
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class BetController : ControllerBase
    {
        private readonly ILogger<BetController> _logger;
        private readonly IBetService _betService;

        public BetController(ILogger<BetController> logger, IBetService betService)
        {
            _logger = logger;
            _betService = betService;
        }

        [HttpPost]
        [Route("createBet")]
        public async Task<IActionResult> CreateBet(BetDto bet)
        {
            try
            {
                _logger.LogInformation("Creating new bet.");
                string username = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                int id = await _betService.CreateBet(bet, username);
                _logger.LogInformation($"Bet {id} created.");
                return Created(string.Empty, $"Apuesta creada con éxito.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating bet. {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
