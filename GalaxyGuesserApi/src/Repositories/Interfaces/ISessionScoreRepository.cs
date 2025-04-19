using System.Threading.Tasks;
using GalaxyGuesserApi.Models;
using static GalaxyGuesserApi.Models.SessionScore;

namespace GalaxyGuesserApi.Repositories.Interfaces
{
    public interface ISessionScoreRepository
    {
        Task<int?> GetPlayerScoreAsync(int? playerId, int sessionId);
        Task UpdatePlayerScoreAsync(int? playerId, int sessionId, int points);
        Task<SessionStats> GetSessionStatsAsync(int playerId, int sessionId);
    }
}
