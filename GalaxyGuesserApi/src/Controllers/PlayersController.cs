using Microsoft.AspNetCore.Mvc;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Services;

namespace GalaxyGuesserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerService _PlayerService;

        public PlayersController(PlayerService PlayerService)
        {
            _PlayerService = PlayerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            try
            {
                var Players = await _PlayerService.GetAllPlayersAsync();
                return Ok(Players);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            try
            {
                var Player = await _PlayerService.GetPlayerByIdAsync(id);
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
                var player = await _PlayerService.CreatePlayerAsync(guid, username);
                return CreatedAtAction(nameof(GetPlayer), new { id = player.player_id }, player);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, Player Player)
        {
            if (id != Player.player_id)
            {
                return BadRequest();
            }

            try
            {
                await _PlayerService.UpdatePlayerAsync(Player);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            try
            {
                await _PlayerService.DeletePlayerAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}