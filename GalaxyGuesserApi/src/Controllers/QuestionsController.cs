using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GalaxyGuesserApi.Models;
using System.Security.Claims;
using GalaxyGuesserApi.Services;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using GalaxyGuesserApi.Models;
using System.Collections.Generic;
using System;

namespace GalaxyGuesserApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  //[Authorize]
  public class QuestionsController : ControllerBase
  {
    private readonly QuestionService _questionsService;

    public QuestionsController(QuestionService questionsService)
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
          return NotFound(new { message = "No question found for this session." });

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
      try
      {
        var options = await _questionsService.GetOptionsByQuestionIdAsync(questionId);
        if (options == null || options.Count == 0)
          return NotFound("No options found for that question.");

        return Ok(options);
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
          return NotFound(new { message = "Answer not found." });

        return Ok(answer);
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Internal server error: {ex.Message}");
      }
    }
  }
}