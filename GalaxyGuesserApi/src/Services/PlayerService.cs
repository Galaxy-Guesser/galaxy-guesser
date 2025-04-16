using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;

namespace GalaxyGuesserApi.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _playerRepository.GetAllPlayersAsync();
        }

        public async Task<Player> GetPlayerByIdAsync(int player_id)
        {
            return await _playerRepository.GetPlayerByIdAsync(player_id);
        }

        public async Task<Player?> GetPlayerByGuidAsync(string guid)
        {
            return await _playerRepository.GetPlayerByGuidAsync(guid);
        }

        public async Task<Player> CreatePlayerAsync(string guid, string username)
        {
           return await _playerRepository.CreatePlayerAsync(guid, username);
        }

        public async Task<bool> UpdatePlayerAsync(int player_id, string username)
        {
            var existingPlayer = await _playerRepository.GetPlayerByIdAsync(player_id);
            if (existingPlayer == null)
            {
                return false;
            }

            return await _playerRepository.UpdatePlayerAsync(player_id, username);
        }

        public async Task<bool> DeletePlayerAsync(int player_id)
        {
           var existingPlayer = await _playerRepository.GetPlayerByIdAsync(player_id);
            if (existingPlayer == null)
            {
                return false;
            }

            return await _playerRepository.DeletePlayerAsync(player_id);
        }


    }
}