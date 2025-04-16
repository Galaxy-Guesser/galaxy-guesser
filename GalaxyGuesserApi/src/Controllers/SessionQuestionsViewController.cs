using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Services;

namespace GalaxyGuesserApi.Controllers
{
    [ApiController]
    [Authorize] 
    [Route("api/sessionQuestions")]
    public class SessionQuestionsViewController : ControllerBase
    {
        private readonly SessionQuestionsViewService _sessionQuestionViewService;

        public SessionQuestionsViewController(SessionQuestionsViewService sessionQuestionViewService)
        {
            _sessionQuestionViewService = sessionQuestionViewService;
        }

       [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionQuestionView>>> GetAllSessionQuestions([FromQuery] string sessionCode)
        {
            try
            {
                var sessionQuestions = await _sessionQuestionViewService.GetAllSessionQuestions(sessionCode);
                return Ok(sessionQuestions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
