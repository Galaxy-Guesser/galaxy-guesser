using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;

namespace GalaxyGuesserApi.Services
{
    public class PlayerService
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

        public async Task<Player?> GetUserByGoogleIdAsync(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                throw new ArgumentException("Google ID cannot be null or empty");

            return await _playerRepository.GetUserByGoogleIdAsync(guid);
        }

        public async Task<Player?> GetPlayerByIdAsync(int playerId)
        {
            if (playerId <= 0)
                throw new ArgumentException("Player ID must be positive");

            return await _playerRepository.GetPlayerByIdAsync(playerId);
        }

        public async Task<Player> CreatePlayerAsync(string guid, string username)
        {
            return await _playerRepository.CreatePlayerAsync(guid, username);
        }

        public async Task<bool> UpdatePlayerAsync(int playerId, string username)
        {
            if (playerId <= 0)
                throw new ArgumentException("Player ID must be positive");

            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username cannot be null or empty");

            var existingPlayer = await _playerRepository.GetPlayerByIdAsync(playerId);
            if (existingPlayer == null)
            {
                return false;
            }

            return await _playerRepository.UpdatePlayerAsync(playerId, username);
        }

        public async Task<bool> DeletePlayerAsync(int playerId)
        {
            if (playerId <= 0)
                throw new ArgumentException("Player ID must be positive");

            var existingPlayer = await _playerRepository.GetPlayerByIdAsync(playerId);
            if (existingPlayer == null)
            {
                return false;
            }

            return await _playerRepository.DeletePlayerAsync(playerId);
        }
    }
}