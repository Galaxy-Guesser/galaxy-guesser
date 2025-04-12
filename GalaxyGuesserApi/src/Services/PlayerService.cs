using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;

namespace GalaxyGuesserApi.Services
{
    public class PlayerService
    {
        private readonly IPlayerRepository _PlayerRepository;

        public PlayerService(IPlayerRepository PlayerRepository)
        {
            _PlayerRepository = PlayerRepository;
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _PlayerRepository.GetAllPlayersAsync();
        }

        public async Task<Player> GetPlayerByIdAsync(int player_id)
        {
            return await _PlayerRepository.GetPlayerByIdAsync(player_id);
        }

        public async Task<Player> CreatePlayerAsync(string guid, string username)
        {
           return await _PlayerRepository.CreatePlayerAsync(guid, username);
        }

        public async Task<bool> UpdatePlayerAsync(int player_id, string username)
        {
            var existingPlayer = await _PlayerRepository.GetPlayerByIdAsync(player_id);
            if (existingPlayer == null)
            {
                return false; // Player not found
            }

            return await _PlayerRepository.UpdatePlayerAsync(player_id, username);
        }

        public async Task<bool> DeletePlayerAsync(int player_id)
        {
           var existingPlayer = await _PlayerRepository.GetPlayerByIdAsync(player_id);
            if (existingPlayer == null)
            {
                return false; // Player not found
            }

            return await _PlayerRepository.DeletePlayerAsync(player_id);
        }
    }
}