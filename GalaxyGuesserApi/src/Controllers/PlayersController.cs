using Microsoft.AspNetCore.Mvc;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Services;

namespace GalaxyGuesserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerService _playerService;

        public PlayersController(PlayerService playerService)
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

        [HttpGet("{player_id}")]
        public async Task<ActionResult<Player>> GetPlayer(int player_id)
        {
            try
            {
                var Player = await _playerService.GetPlayerByIdAsync(player_id);
                if (Player == null)
                {
                    return NotFound();
                }
                return Ok(Player);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Player>> CreatePlayer(string guid, string username)
        {
            try
            {
                var player = await _playerService.CreatePlayerAsync(guid, username);
                return CreatedAtAction(nameof(GetPlayer), new { id = player.player_id }, player);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{player_id}")]
        public async Task<IActionResult> UpdatePlayer(int player_id, [FromBody] Player player)
        {
           if (player_id != player.player_id)
            {
                return BadRequest("Player ID in the URL does not match the ID in the request body.");
            }

            try
            {
                var updated = await _playerService.UpdatePlayerAsync(player_id, player.username);

                if (!updated)
                {
                    return NotFound($"Player with ID {player_id} not found.");
                }

                return Ok(new
                {
                    message = "Player updated successfully",
                    player_id,
                    updated_username = player.username
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{player_id}")]
        public async Task<IActionResult> DeletePlayer(int player_id)
        {
            try
                {
                    var deleted = await _playerService.DeletePlayerAsync(player_id);

                    if (!deleted)
                    {
                        return NotFound($"Player with ID {player_id} not found.");
                    }

                     return Ok(new { message = "User deleted successfully" });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
        }
    }
}