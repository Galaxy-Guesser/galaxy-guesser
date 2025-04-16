
using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Services
{
    public interface IPlayerService
    {
        Task<List<Player>> GetAllPlayersAsync();
        Task<Player> GetPlayerByIdAsync(int player_id);
        Task<Player> GetPlayerByGuid(string guid);
        Task<Player> CreatePlayerAsync(string guid, string username);
        Task<bool> UpdatePlayerAsync(int player_id, string username);
        Task<Player?> GetPlayerByGuidAsync(string guid);
        Task<bool> DeletePlayerAsync(int player_id);
    }
}