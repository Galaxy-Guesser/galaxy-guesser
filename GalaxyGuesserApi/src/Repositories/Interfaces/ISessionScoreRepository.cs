using static GalaxyGuesserApi.Models.SessionScore;

namespace GalaxyGuesserApi.Repositories.Interfaces
{
    public interface ISessionScoreRepository
    {
        Task<bool> IsPlayerInSessionAsync(int playerId, int sessionId);
        Task<bool> IsQuestionInSessionAsync(int questionId, int sessionId);
        Task<int> GetCorrectAnswerIdAsync(int questionId);
        Task<int?> GetPlayerScoreAsync(int playerId, int sessionId);
        Task UpdatePlayerScoreAsync(int playerId, int sessionId, int points);
        Task AddPlayerScoreAsync(int playerId, int sessionId, int points);
        Task<SessionStats> GetSessionStatsAsync(int playerId, int sessionId);
        Task<string> GetSessionCategoryAsync(int sessionId);
    }
}