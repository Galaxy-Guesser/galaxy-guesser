using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;

namespace GalaxyGuesserApi.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerService _PlayerService;

        public PlayerService(IPlayerService PlayerService)
        {
            _PlayerService = PlayerService;
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _PlayerService.GetAllPlayersAsync();
        }

        public async Task<Player> GetPlayerByIdAsync(int player_id)
        {
            return await _PlayerService.GetPlayerByIdAsync(player_id);
        }

        public async Task<Player?> GetPlayerByGuidAsync(string guid)
        {
            return await _PlayerService.GetPlayerByGuidAsync(guid);
        }

        public async Task<Player> CreatePlayerAsync(string guid, string username)
        {
           return await _PlayerService.CreatePlayerAsync(guid, username);
        }

        public async Task<bool> UpdatePlayerAsync(int player_id, string username)
        {
            var existingPlayer = await _PlayerService.GetPlayerByIdAsync(player_id);
            if (existingPlayer == null)
            {
                return false; // Player not found
            }

            return await _PlayerService.UpdatePlayerAsync(player_id, username);
        }

        public async Task<bool> DeletePlayerAsync(int player_id)
        {
           var existingPlayer = await _PlayerService.GetPlayerByIdAsync(player_id);
            if (existingPlayer == null)
            {
                return false; // Player not found
            }

            return await _PlayerService.DeletePlayerAsync(player_id);
        }


    }
}