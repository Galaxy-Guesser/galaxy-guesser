using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GalaxyGuesserApi.Models;
using System.Security.Claims; // Add this line
using GalaxyGuesserApi.Services;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks; // Add this line
using GalaxyGuesserApi.Models;
using System.Collections.Generic;

namespace GalaxyGuesserApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize]
  public class QuestionsController : ControllerBase
  {
    private readonly QuestionService _questionsService;

    public QuestionsController(QuestionService questionsService)
    {
      _questionsService = questionsService;
    }

    //[HttpGet("{sessionId}")]
    //public async Task<ActionResult<Question>> GetQuestionsBySessionId(int sessionId)
    //{
    //  try
    //  {
    //    Question question = await _questionsService.GetQuestionsBySessionIdAsync(questionId);
    //    if (question == null)
    //    {
    //      return NotFound();
    //    }
    //    return Ok(question);
    //  }
    //  catch (Exception ex)
    //  {
    //    return StatusCode(500, $"Internal server error: {ex.Message}");
    //  }
    //}

    [HttpGet("{questionId}")]
    public async Task<ActionResult<Question>> GetQuestion(int questionId)
    {
      try
      {
        Question question = await _questionsService.GetQuestionAsync(questionId);
        if (question == null)
        {
          return NotFound();
        }
        return Ok(question);
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Internal server error: {ex.Message}");
      }
    }

    [HttpGet("{questionId}/options")]
    public async Task<IActionResult> GetOptions(int questionId)
    {
      var options = await _questionsService.GetOptionsByQuestionIdAsync(questionId);

      if (options == null || options.Count == 0)
        return NotFound("No options found for that question.");

      return Ok(options);
    }

    [HttpGet]
    public async Task<ActionResult<List<Question>>> GetQuestions()
    {
      var questions = await _questionsService.GetQuestionsAsync();

      if (questions == null || questions.Count == 0)
        return NotFound("No questions found.");

      return Ok(questions);
    }

    //[HttpPost]
    //public async Task<ActionResult<Question>> CreateQuestion()
  }
}