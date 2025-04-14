using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaxyGuesserApi.src.Models;
using GalaxyGuesserApi.src.Services;
using Microsoft.AspNetCore.Mvc;

namespace GalaxyGuesserApi.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionScoreController : ControllerBase
    {
        private readonly SessionScoreService _sessionScoreService;

        public SessionScoreController(SessionScoreService sessionScoreService)
        {
            _sessionScoreService = sessionScoreService;
        }


        [HttpPut("{sessionId:int}/{playerId:int}")]
        public async Task<IActionResult> UpdateScore(int sessionId, int playerId, [FromBody] SessionScore request)
        {
            var updated = await _sessionScoreService.UpdatePlayerScoreAsync(sessionId, playerId, request.score);

            if (!updated)
                return NotFound($"No score found for session {sessionId} and player {playerId}");

            return Ok("Score updated successfully.");
        }
    }
}