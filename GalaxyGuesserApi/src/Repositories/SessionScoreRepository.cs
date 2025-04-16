using GalaxyGuesserApi.Data;
using static GalaxyGuesserApi.Models.SessionScore;
using GalaxyGuesserApi.Repositories.Interfaces;

namespace GalaxyGuesserApi.Repositories
{
    public class SessionScoreRepository : ISessionScoreRepository
    {
        private readonly DatabaseContext _dbContext;

        public SessionScoreRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task<int?> GetPlayerScoreAsync(int playerId, int sessionId)
        {
            var result = await _dbContext.QueryAsync<int>(
                "SELECT score FROM SessionScores WHERE player_id = @playerId AND session_id = @sessionId",
                reader => reader.GetInt32(0),
                new Dictionary<string, object>
                {
                    { "playerId", playerId },
                    { "sessionId", sessionId }
                });
            return result.FirstOrDefault();
        }

        public async Task<SessionStats> GetSessionStatsAsync(int playerId, int sessionId)
        {
            var totalScore = await GetPlayerScoreAsync(playerId, sessionId) ?? 0;
            
            return new SessionStats(
                TotalScore: totalScore);
        }

        public async Task UpdatePlayerScoreAsync(int playerId, int sessionId, int points)
        {
            await _dbContext.ExecuteNonQueryAsync(
                "UPDATE SessionScores SET score = score + @points WHERE player_id = @playerId AND session_id = @sessionId",
                new Dictionary<string, object>
                {
                    { "points", points },
                    { "playerId", playerId },
                    { "sessionId", sessionId }
                });

        }
    }
}