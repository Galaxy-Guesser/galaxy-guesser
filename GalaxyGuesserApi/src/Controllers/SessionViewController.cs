using Microsoft.AspNetCore.Mvc;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Services;

namespace GalaxyGuesserApi.Controllers
{
    [ApiController]
    [Route("api/change")]
    public class SessionViewController : ControllerBase
    {
        private readonly SessionViewService _sessionViewService;

        public SessionViewController(SessionViewService sessionViewService)
        {
            _sessionViewService = sessionViewService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionView>>> GetAllActiveSessions()
        {
            try
            {
                var sessions = await _sessionViewService.GetAllActiveSessions();
                return Ok(sessions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
