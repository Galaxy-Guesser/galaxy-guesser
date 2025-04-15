using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Services;
using Microsoft.AspNetCore.Mvc;
using static GalaxyGuesserApi.Models.SessionScore;

namespace GalaxyGuesserApi.Controllers
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


        [HttpPut("update")]
        public async Task<IActionResult> UpdateScore([FromBody] ScoreUpdateRequest request)
        {
            var response = await _sessionScoreService.UpdateScoreAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("finalScore/{sessionId}/{playerId}")]
        public async Task<IActionResult> GetFinalScore(int sessionId, int playerId)
        {
            var response = await _sessionScoreService.GetFinalScoreAsync(playerId, sessionId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}