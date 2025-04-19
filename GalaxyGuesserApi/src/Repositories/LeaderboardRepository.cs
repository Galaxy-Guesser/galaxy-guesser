using GalaxyGuesserApi.Data;
using GalaxyGuesserApi.Repositories.Interfaces;
using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Repositories
{
    public class LeaderboardRepository : ILeaderboardRepository
    {
        private readonly DatabaseContext _dbContext;

        public LeaderboardRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
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