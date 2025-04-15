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

        public async Task AddPlayerScoreAsync(int playerId, int sessionId, int points)
        {
            await _dbContext.ExecuteNonQueryAsync(
                "INSERT INTO SessionScores (player_id, session_id, score) VALUES (@playerId, @sessionId, @points)",
                new Dictionary<string, object>
                {
                    { "playerId", playerId },
                    { "sessionId", sessionId },
                    { "points", points }
                });
        }

        public async Task<int> GetCorrectAnswerIdAsync(int questionId)
        {
            var result = await _dbContext.QueryAsync<int>(
                "SELECT answer_id FROM Questions WHERE question_id = @questionId",
                reader => reader.GetInt32(0),
                new Dictionary<string, object> { { "questionId", questionId } });
            return result.FirstOrDefault();
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
        public async Task<string> GetSessionCategoryAsync(int sessionId)
        {
            var result = await _dbContext.QueryAsync<string>(
                @"SELECT c.category FROM Sessions s 
                JOIN Categories c ON s.category_id = c.category_id 
                WHERE s.session_id = @sessionId",
                reader => reader.GetString(0),
                new Dictionary<string, object> { { "sessionId", sessionId } });
            return result.FirstOrDefault() ?? "Unknown";
        }

        public async Task<SessionStats> GetSessionStatsAsync(int playerId, int sessionId)
        {
            var totalScore = await GetPlayerScoreAsync(playerId, sessionId) ?? 0;
            
            return new SessionStats(
                TotalScore: totalScore);
        }

        public async Task<bool> IsPlayerInSessionAsync(int playerId, int sessionId)
        {
            var result = await _dbContext.QueryAsync<bool>(
            "SELECT EXISTS(SELECT 1 FROM SessionPlayers WHERE player_id = @playerId AND session_id = @sessionId)",
                reader => reader.GetBoolean(0),
                new Dictionary<string, object>
                {
                    { "playerId", playerId },
                    { "sessionId", sessionId }
                });
            return result.FirstOrDefault();
        }

        public async Task<bool> IsQuestionInSessionAsync(int questionId, int sessionId)
        {
             var result = await _dbContext.QueryAsync<bool>(
            "SELECT EXISTS(SELECT 1 FROM SessionQuestions WHERE question_id = @questionId AND session_id = @sessionId)",
                reader => reader.GetBoolean(0),
                new Dictionary<string, object>
                {
                    { "questionId", questionId },
                    { "sessionId", sessionId }
                });
            return result.FirstOrDefault();
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