using GalaxyGuesserApi.Services;
using Microsoft.AspNetCore.Mvc;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Services.Interfaces;


namespace GalaxyGuesserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

         [HttpGet("session/{sessionCode}")]
        public async Task<ActionResult<SessionLeaderboardResponse>> GetSessionLeaderboard(string sessionCode)
        {
            var response = await _leaderboardService.GetSessionLeaderboardAsync(sessionCode);
            
            if (!response.Success)
            {
                return NotFound(response);
            }
            
            return Ok(response);
        }

        [HttpGet("global")]
        public async Task<ActionResult<GlobalLeaderboardResponse>> GetGlobalLeaderboard()
        {
            var response = await _leaderboardService.GetGlobalLeaderboardAsync();
            
            if (!response.Success)
            {
                return NotFound(response);
            }
            
            return Ok(response);
        }
    }
}