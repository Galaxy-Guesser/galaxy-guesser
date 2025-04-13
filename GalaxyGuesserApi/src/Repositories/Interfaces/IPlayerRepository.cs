using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Repositories.Interfaces
{
    public interface IPlayerRepository
    {
        Task<List<Player>> GetAllPlayersAsync();
        Task<Player> GetPlayerByIdAsync(int playerId);
        Task<Player> GetUserByGoogleIdAsync (string guid);
        Task<Player> CreatePlayerAsync(string guid, string userName);
        Task<bool> UpdatePlayerAsync(int playerId, string userName);
        Task<bool> DeletePlayerAsync(int playerId);

    }
}