using GalaxyGuesserApi.Data;
using static GalaxyGuesserApi.Models.SessionScore;
using GalaxyGuesserApi.Repositories.Interfaces;
using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Repositories
{
    public class SessionScoreRepository : ISessionScoreRepository
    {
        private readonly DatabaseContext _dbContext;

        public SessionScoreRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task<int?> GetPlayerScoreAsync(int? playerId, int sessionId)
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

        public async Task UpdatePlayerScoreAsync(int? playerId, int sessionId, int points)
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

         public async Task<List<LeaderboardEntry>> GetSessionLeaderboardAsync(string sessionCode)
        {
            string sql = @"SELECT * FROM SessionLeaderboardView WHERE session_code= @sessionCode";

             var parameters = new Dictionary<string, object>
            {
                { "@sessionCode", sessionCode }
            };

            return await _dbContext.QueryAsync<LeaderboardEntry>(
                sql,
                reader => new LeaderboardEntry
                {
                    SessionCode = reader.GetString(reader.GetOrdinal("session_code")),
                    UserName = reader.GetString(reader.GetOrdinal("user_name")),
                    Score = reader.GetInt32(reader.GetOrdinal("score")),
                    Rank = reader.GetInt32(reader.GetOrdinal("rank"))
                },
                parameters
            );
        }
        public async Task<List<GlobalLeaderboardEntry>> GetGlobalLeaderboardAsync()
        {
            string sql = @" SELECT * FROM GlobalLeaderboardView";

            return await _dbContext.QueryAsync<GlobalLeaderboardEntry>(
                sql,
                reader => new GlobalLeaderboardEntry
                {
                    PlayerId = reader.GetInt32(reader.GetOrdinal("player_id")),
                    UserName = reader.GetString(reader.GetOrdinal("user_name")),
                    TotalScore = reader.GetInt32(reader.GetOrdinal("total_score")),
                    SessionsPlayed = reader.GetInt32(reader.GetOrdinal("sessions_played")),
                    Rank = reader.GetInt32(reader.GetOrdinal("rank")),
                    Sessions = reader.IsDBNull(reader.GetOrdinal("sessions")) 
                    ? new List<string>() 
                    : ((string[])reader.GetValue(reader.GetOrdinal("sessions"))).ToList()
                },
                null
            );
        }
    }
}