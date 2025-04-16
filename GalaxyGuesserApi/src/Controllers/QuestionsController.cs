using Microsoft.AspNetCore.Mvc;
using GalaxyGuesserApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace GalaxyGuesserApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize]
  public class QuestionsController : ControllerBase
  {
    private readonly IQuestionService _questionsService;

    public QuestionsController(IQuestionService questionsService)
    {
      _questionsService = questionsService;
    }

     [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionQuestionView>>> GetAllSessionQuestions([FromQuery] int categoryId)
        {
            try
            {
                var sessionQuestions = await _questionsService.GetQuestionCountForCategory(categoryId);
                return Ok(sessionQuestions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
   
   
  }
}