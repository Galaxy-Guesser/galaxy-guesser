using GalaxyGuesserApi.Data;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;

namespace GalaxyGuesserApi.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DatabaseContext _dbContext;

        public SessionRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task JoinSessionAsync(string sessionCode, string playerGuid)
        {
            const string sql = "CALL join_session(@session_code, @player_guid)";
            var parameters = new Dictionary<string, object>
            {
                { "@session_code", sessionCode },
                { "@player_guid",playerGuid }
            };

            await _dbContext.ExecuteNonQueryAsync(sql, parameters);
        }

        public async Task<List<SessionDTO>> GetAllSessionsAsync()
        {
            const string sql = "SELECT session_code, " +
                              "category, " +
                              "player.user_name" +
                              "FROM sessions " +
                              "INNER JOIN categories ON sessions.category_id = categories.category_id " +
                              "INNER JOIN players on player.guid = session.player_guid";

            return await _dbContext.QueryAsync(sql, reader => new SessionDTO
            {
                sessionCode = reader.GetString(0),
                category = reader.GetString(1),
                user_name = reader.GetString(2),
            });
        }
        public async Task CreateSessionAsync(string category, int questionsCount, string user_guid)
        {
            const string sql = "CALL create_session (@category,@user_guid,@question_count)";
            var parameters = new Dictionary<string, object>
            {
                { "@category", category },
                { "@question_count", questionsCount },
                { "@user_guid", user_guid },
            };

            await _dbContext.ExecuteNonQueryAsync(sql, parameters);
        }

        public async Task<SessionDTO> GetSessionByCodeAsync(string sessionCode)
        {
            const string sql = "SELECT session_code, " +
                "category, " +
                "user_name " +
                "FROM sessions " +
                "INNER JOIN categories ON sessions.category_id = categories.category_id " +
                "INNER JOIN players ON players.guid = session.player_guid "+
                "WHERE sessions.session_code= @session_code";
            var parameters = new Dictionary<string, object> { { "@session_code", sessionCode } };
            var sessions = await _dbContext.QueryAsync(sql, reader => new SessionDTO
            {
                sessionCode = reader.GetString(0),
                category = reader.GetString(1),
                user_name = reader.GetString(3),
            }, parameters);

            return sessions.FirstOrDefault()!;
        }

        public async Task UpdateSessionAsync(SessionDTO session)
        {
            const string sql = "UPDATE sessions SET name = @name, email = @email WHERE id = @id";
            var parameters = new Dictionary<string, object>
            {
                // { "@id", Player.player_id },
                // { "@name", Player.username },
            };

            await _dbContext.ExecuteNonQueryAsync(sql, parameters);
        }

        public async Task DeleteSessionAsync(string session_code)
        {
            const string sql = "Update sessions set end_date=now() WHERE session_code = @session_code";
            var parameters = new Dictionary<string, object> { { "@id", session_code } };

            await _dbContext.ExecuteNonQueryAsync(sql, parameters);
        }
    }
}