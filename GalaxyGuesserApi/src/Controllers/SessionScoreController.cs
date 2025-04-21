using GalaxyGuesserApi.Services;
using Microsoft.AspNetCore.Mvc;
using static GalaxyGuesserApi.Models.SessionScore;
using Microsoft.AspNetCore.Authorization;
using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/session-scores")]
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

         [HttpGet("{sessionCode}")]
        public async Task<ActionResult<SessionLeaderboardResponse>> GetSessionLeaderboard(string sessionCode)
        {
            try
            {
                var response = await _sessionScoreService.GetSessionLeaderboardAsync(sessionCode);
                
                if (!response.Success)
                {
                    return NotFound(response);
                }
                
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, new SessionLeaderboardResponse 
                { 
                    Success = false, 
                    Message = "An error occurred while processing your request." 
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult<GlobalLeaderboardResponse>> GetGlobalLeaderboard()
        {
            try
            {
                var response = await _sessionScoreService.GetGlobalLeaderboardAsync();
                
                if (!response.Success)
                {
                    return NotFound(response);
                }
                
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, new GlobalLeaderboardResponse 
                { 
                    Success = false, 
                    Message = "An error occurred while processing your request." 
                });
            }
        }
    }
}
