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
                playerUserNames = reader.GetFieldValue<List<string>>(3),
                playerCount = reader.GetInt32(4),
                duration = reader.GetString(5),
                questionCount = reader.GetInt32(6),
                highestScore = reader.IsDBNull(7) ? null : reader.GetInt32(7),
                endsIn = reader.IsDBNull(8) ? null : reader.GetString(8)
            });
        }
    }
}
