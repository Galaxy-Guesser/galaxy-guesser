using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Services.Interfaces;

namespace GalaxyGuesserApi.Controllers
{
    [ApiController]
    [Authorize] 
    [Route("api/change")]
    public class SessionViewController : ControllerBase
    {
        private readonly ISessionViewService _sessionViewService;

        public SessionViewController(ISessionViewService sessionViewService)
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
