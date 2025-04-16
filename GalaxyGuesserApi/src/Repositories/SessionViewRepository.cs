using GalaxyGuesserApi.Data;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;

namespace GalaxyGuesserApi.Repositories
{
    public class SessionViewRepository : ISessionViewRepository
    {
         private readonly DatabaseContext _dbContext;

        public SessionViewRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SessionView>> GetAllActiveSessions()
        {
            const string sql = "SELECT * FROM view_active_sessions";

            return await _dbContext.QueryAsync(sql, reader => new SessionView
            {
                sessionId = reader.GetInt32(0),
                sessionCode = reader.GetString(1),
                category = reader.GetString(2),
                playerUserNames = reader.IsDBNull(3) ? new List<string>() : reader.GetFieldValue<List<string>>(3),
                playerCount = reader.GetInt32(4),
                questionCount = reader.GetInt32(5),
                highestScore = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                endsIn = reader.IsDBNull(7) ? null : reader.GetString(7)
            });
        }
    }
}
