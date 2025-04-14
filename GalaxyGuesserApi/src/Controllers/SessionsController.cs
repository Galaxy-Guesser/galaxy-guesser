using Microsoft.AspNetCore.Mvc;
using GalaxyGuesserApi.Models;
using  GalaxyGuesserApi.Services;

namespace GalaxyGuesserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionsController : ControllerBase
    {
        private readonly SessionService _sessionService;

        public SessionsController(SessionService sessionService)
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
                await _sessionService.CreateSessionAsync(request);
                return Ok("created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionDTO>>> GetSessions()
        {
            try
            {
                var sessions = await _sessionService.GetAllSessionsAsync();
                return Ok(sessions);
            }catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error {ex.Message}"); 
            }
        }

        [HttpPost]
public async Task<IActionResult> JoinSession([FromBody] JoinSessionRequest request)
{
    try
    {
        await _sessionService.JoinSessionAsync(request.sessionCode, request.playerGuid);
        return Ok("Player successfully joined the session.");
    }
    catch (Exception ex)
    {
        return BadRequest($"Join failed: {ex.Message}");
    }
}
    }
}