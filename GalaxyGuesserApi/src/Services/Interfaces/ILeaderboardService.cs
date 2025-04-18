using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Services.Interfaces
{
    public interface ILeaderboardService
    {
        Task<SessionLeaderboardResponse> GetSessionLeaderboardAsync(string sessionCode);
        Task<GlobalLeaderboardResponse> GetGlobalLeaderboardAsync();
    }
}