using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Repositories.Interfaces
{
    public interface ILeaderboardRepository
    {
        Task<List<LeaderboardEntry>> GetSessionLeaderboardAsync(string sessionCode);
        Task<List<GlobalLeaderboardEntry>> GetGlobalLeaderboardAsync();
    }
}