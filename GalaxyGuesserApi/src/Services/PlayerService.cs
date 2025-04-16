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

        public async Task<Player> GetPlayerByIdAsync(int playerId)
        {
            return await _playerRepository.GetPlayerByIdAsync(playerId);
        }

        public async Task<Player> CreatePlayerAsync(string guid, string userName)
        {
           return await _playerRepository.CreatePlayerAsync(guid, userName);
        }

        public async Task<bool> UpdatePlayerAsync(int playerId, string userName)
        {
            var existingPlayer = await _playerRepository.GetPlayerByIdAsync(playerId);
            if (string.IsNullOrEmpty(existingPlayer.ToString()))
            {
                return false;
            }

            return await _playerRepository.UpdatePlayerAsync(playerId, userName);
        }

        public async Task<Player?> GetPlayerByGuidAsync(string guid)
        {
            return await _playerRepository.GetPlayerByGuidAsync(guid);
        }

        public async Task<bool> DeletePlayerAsync(int playerId)
        {
           var existingPlayer = await _playerRepository.GetPlayerByIdAsync(playerId);
            if (string.IsNullOrEmpty(existingPlayer.ToString()))
            {
                return false;
            }

            return await _playerRepository.DeletePlayerAsync(playerId);
        }
    }
}