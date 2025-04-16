using System.Threading.Tasks;
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

        [HttpPut]
        public async Task<IActionResult> UpdateScore([FromBody] ScoreUpdateRequest request)
        {
            var response = await _sessionScoreService.UpdateScoreAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
