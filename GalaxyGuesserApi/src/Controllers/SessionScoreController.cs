using GalaxyGuesserApi.Services;
using Microsoft.AspNetCore.Mvc;
using static GalaxyGuesserApi.Models.SessionScore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GalaxyGuesserApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SessionScoreController : BaseController
    {
        private readonly SessionScoreService _sessionScoreService;

        private readonly IPlayerService _playerService;

        public SessionScoreController(SessionScoreService sessionScoreService, IPlayerService playerService)
        {
            _sessionScoreService = sessionScoreService;

            _playerService = playerService;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateScore([FromBody] ScoreUpdateRequest request)
        {
            var googleId = GetGoogleIdFromClaims();
            var player = await _playerService.GetPlayerByGuidAsync(googleId);
            if(player!=null){
                var response = await _sessionScoreService.UpdateScoreAsync(request, player?.playerId);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            return BadRequest("No player found");
        }
    }
}
