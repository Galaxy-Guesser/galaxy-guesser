using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GalaxyGuesserApi.Models;
using System.Security.Claims;
using GalaxyGuesserApi.Services;

namespace GalaxyGuesserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            try
            {
                var Players = await _playerService.GetAllPlayersAsync();
                return Ok(Players);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{playerId}")]
        public async Task<ActionResult<Player>> GetPlayer(int playerId)
        {
            try
            {
                var Player = await _playerService.GetPlayerByIdAsync(playerId);
                if (Player == null)
                {
                    return NotFound();
                }
                else{
                    return Ok(Player);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Player>> CreatePlayer(string guid, string userName)
        {
            try
            {
                var player = await _playerService.CreatePlayerAsync(guid, userName);
                return CreatedAtAction(nameof(GetPlayer), new { id = player.playerId }, player);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{playerId}")]
        public async Task<IActionResult> UpdatePlayer(int playerId, [FromBody] Player player)
        {
            
           if (playerId != player.playerId)
            {
                return BadRequest("Player ID in the URL does not match the ID in the request body.");
            }
            else
            {
                try
                {
                    var updated = await _playerService.UpdatePlayerAsync(playerId, player.userName);

                    if (!updated)
                    {
                        return NotFound($"Player with ID {playerId} not found.");
                    }
                    else
                    {
                        return Ok(new
                        {
                            message = "Player updated successfully",
                            playerId,
                            updated_username = player.userName
                        });
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }

        [Authorize] 
        [HttpPost("auth")]
        public async Task<ActionResult<Player>> AuthenticateOrRegister([FromBody] string? displayName = null)
        {
            var googleId = User.FindFirst("sub")?.Value 
                        ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst("email")?.Value;

            if (string.IsNullOrEmpty(googleId))
            {
                return Unauthorized(new {
                    Message = "Google ID (sub claim) missing",
                    AllClaims = User.Claims.Select(c => new { c.Type, c.Value })
                });
            }
            else
            {
                var player = await _playerService.GetPlayerByGuidAsync(googleId);

                if (player != null)
                {
                    return Ok(player); 
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(displayName))
                    {
                        return BadRequest("Display name required for new users.");
                    }
                    else
                    {
                        player = await _playerService.CreatePlayerAsync(googleId, displayName);
                        return player;
                    }
                }
            }
        }


        [HttpDelete("{playerId}")]
        public async Task<IActionResult> DeletePlayer(int playerId)
        {
            try
                {
                    var deleted = await _playerService.DeletePlayerAsync(playerId);

                    if (!deleted)
                    {
                        return NotFound($"Player with ID {playerId} not found.");
                    }
                    else
                    {
                         return Ok(new { message = "User deleted successfully" });
                    }

                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
        }
    }
}