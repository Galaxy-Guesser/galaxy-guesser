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
                session_id = reader.GetInt32(0),
                session_code = reader.GetString(1),
                session_category = reader.GetString(2),
                player_usernames = reader.GetFieldValue<List<string>>(3),
                player_count = reader.GetInt32(4),
                duration = reader.GetString(5),
                question_count = reader.GetInt32(6),
                highest_score = reader.IsDBNull(7) ? null : reader.GetInt32(7),
                ends_in = reader.IsDBNull(8) ? null : reader.GetString(8)
            });
        }
    }
}
