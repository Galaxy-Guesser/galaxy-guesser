using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Repositories.Interfaces
{
    public interface IPlayerRepository
    {
        Task<List<Player>> GetAllPlayersAsync();
        Task<Player> GetPlayerByIdAsync(int player_id);
        Task<Player> GetUserByGoogleIdAsync (string guid);
        Task<Player> CreatePlayerAsync(string guid, string username);
        Task<bool> UpdatePlayerAsync(int player_id, string username);
        Task<bool> DeletePlayerAsync(int player_id);

    }
}