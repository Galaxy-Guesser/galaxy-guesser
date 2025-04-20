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
        private readonly IQuestionService _questionService;

        public SessionsController(ISessionService sessionService, IQuestionService questionService)
        {
            _sessionService = sessionService;
            _questionService = questionService;
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

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Session>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Session>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<Session>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Session>>> CreateSession([FromBody] CreateSessionRequestDTO request)
        {
            var googleId = User.FindFirst("sub")?.Value 
                       ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var availableQuestionsCount = await _questionService.GetQuestionCountForCategory(request.categoryId);
            if (googleId == null)
            {
                return Unauthorized(ApiResponse<string>.ErrorResponse("User not authenticated"));
            }
            else if (request.questionsCount > availableQuestionsCount)
            {
                return BadRequest(ApiResponse<Session>.ErrorResponse($"Not enough questions available for the selected category. Available questions : {availableQuestionsCount}"));
            }
            else
            {
                try
                {
                    Session createdSession = await _sessionService.CreateSessionAsync(request, googleId);
                    return Ok(ApiResponse<Session>.SuccessResponse(createdSession, "Successfully created session "));
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        ApiResponse<Session>.ErrorResponse("Internal server error", new List<string> { ex.Message }));
                }
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> JoinSession([FromBody] JoinSessionRequest requestBody)
        {
            var playerGuid = User.FindFirst("sub")?.Value
                    ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (playerGuid == null)
            {
                return Unauthorized(ApiResponse<string>.ErrorResponse("User not authenticated"));

            }
            else
            {
                try
                {
                    await _sessionService.JoinSessionAsync(requestBody.sessionCode, playerGuid);
                    return Ok(ApiResponse<string>.SuccessResponse("Successfully joined session"));
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                         ApiResponse<Session>.ErrorResponse("Internal server error", new List<string> { ex.Message }));
                }

            }

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionView>>> GetAllSessions(bool getActive = false)
        {
            try
            {
                if (getActive)
                {
                    var activeSessions = await _sessionService.GetAllActiveSessions();
                    return Ok(activeSessions);
                }
                else
                {
                    var allSessions = await _sessionService.GetAllSessionsAsync();
                    return Ok(allSessions);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}