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

    [HttpGet("ask")]
    public async Task<IActionResult> AskQuestion([FromQuery] int sessionId)
    {
      try
      {
          var question = await _questionsService.GetNextQuestionForSessionAsync(sessionId);
          if (question == null)
          {
            return NotFound(new { message = "No question found for this session." });
          }
          else
          {
          return Ok(question);
          }
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Internal server error: {ex.Message}");
      }
    }

    [HttpGet("{questionId}/options")]
    public async Task<IActionResult> GetOptions(int questionId)
    {
      try
      {
          var options = await _questionsService.GetOptionsByQuestionIdAsync(questionId);
          if (options == null || options.Count == 0)
          {
            return NotFound("No options found for that question.");
          }
          else
          {
            return Ok(options);
          }
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Internal server error: {ex.Message}");
      }
    }

    [HttpGet("{questionId}/answer")]
    public async Task<IActionResult> GetCorrectAnswer(int questionId)
    {
      try
      {
          var answer = await _questionsService.GetCorrectAnswerAsync(questionId);
          if (answer == null)
          {
            return NotFound(new { message = "Answer not found." });
          }
          else
          {
            return Ok(answer);
          }

      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Internal server error: {ex.Message}");
      }
    }
  }
}