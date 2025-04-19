using GalaxyGuesserApi.Services;
using Microsoft.AspNetCore.Mvc;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace GalaxyGuesserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
            try
            {
                var response = await _leaderboardService.GetSessionLeaderboardAsync(sessionCode);
                
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

        [HttpGet("global")]
        public async Task<ActionResult<GlobalLeaderboardResponse>> GetGlobalLeaderboard()
        {
            try
            {
                var response = await _leaderboardService.GetGlobalLeaderboardAsync();
                
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