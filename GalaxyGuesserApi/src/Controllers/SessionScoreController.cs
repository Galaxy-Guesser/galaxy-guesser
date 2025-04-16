using GalaxyGuesserApi.Services;
using Microsoft.AspNetCore.Mvc;
using static GalaxyGuesserApi.Models.SessionScore;
using System.Security.Claims;
using GalaxyGuesserApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace GalaxyGuesserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SessionScoreController : ControllerBase
    {
        private readonly SessionScoreService _sessionScoreService;

        public SessionScoreController(SessionScoreService sessionScoreService,PlayerService playerService)
        {
            _sessionScoreService = sessionScoreService;
            _playerService = playerService;

        }

        private readonly PlayerService _playerService;


        [HttpPut]
        public async Task<IActionResult> UpdateScore([FromBody] ScoreUpdateRequest request)
        {
            var playerGuid = User.FindFirst("sub")?.Value 
                    ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
             Player player = await _playerService.GetPlayerByGuidAsync(playerGuid);
            var response = await _sessionScoreService.UpdateScoreAsync(request,player.playerId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
