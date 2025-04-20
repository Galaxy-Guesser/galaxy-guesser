using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GalaxyGuesserApi.Models;
using System.Security.Claims;
using GalaxyGuesserApi.Services.Interfaces;
using GalaxyGuesserApi.Models.DTO;

namespace GalaxyGuesserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 

    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpGet("code")]
        public async Task<ActionResult<Session>> GetSession(String code)
        {
            try
            {
                var session = await _sessionService.GetSessionAsync(code);
                return Ok(session);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error {ex.Message}");
            }
        }

        [HttpPost("session")]
        public async Task<ActionResult<string>> CreateSession([FromBody] CreateSessionRequestDTO request)
        {
            try
            {
                var googleId = User.FindFirst("sub")?.Value 
                        ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _sessionService.CreateSessionAsync(request,googleId);

                return Ok("created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> JoinSession([FromBody] JoinSessionRequest request)
        {
            try
            {
                var playerGuid = User.FindFirst("sub")?.Value 
                    ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                await _sessionService.JoinSessionAsync(request.sessionCode, playerGuid);
                return Ok("Player successfully joined the session.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Join failed: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionView>>> GetAllActiveSessions()
        {
            try
            {
                var sessions = await _sessionService.GetAllActiveSessions();
                return Ok(sessions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("all")] 
        public async Task<ActionResult<List<SessionDTO>>> GetAllSessions()
        {
            try
            {
                var sessions = await _sessionService.GetAllSessionsAsync();
                return Ok(sessions);
            }
            catch (Exception ex)
            {
                 return StatusCode(500, $"Internal server error: {ex.Message}");   
            }
        }
    }
}