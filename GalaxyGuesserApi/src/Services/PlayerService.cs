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

        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            return await _PlayerRepository.GetPlayerByIdAsync(id);
        }

        public async Task<Player> CreatePlayerAsync(string guid, string username)
        {
           return await _PlayerRepository.CreatePlayerAsync(guid, username);
        }

        public async Task UpdatePlayerAsync(Player Player)
        {
            await _PlayerRepository.UpdatePlayerAsync(Player);
        }

        public async Task DeletePlayerAsync(int id)
        {
            await _PlayerRepository.DeletePlayerAsync(id);
        }
    }
}