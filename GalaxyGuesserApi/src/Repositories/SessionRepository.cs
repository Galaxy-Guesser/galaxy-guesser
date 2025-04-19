using GalaxyGuesserApi.Data;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Models.DTO;
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
            const string sql = "CALL join_session(@sessionCode, @playerGuid)";
            var parameters = new Dictionary<string, object>
            {
                { "@sessionCode", sessionCode },
                { "@playerGuid",playerGuid }
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
                userName = reader.GetString(2),
            });
        }
        public async Task CreateSessionAsync(CreateSessionRequestDTO requestBody,string loggedInUserGuid)
        {
            const string sql = "CALL create_session (@category,@userGuid,@startDate,@sessionDuration,@questionCount)";
            var parameters = new Dictionary<string, object>
             {
                { "@category", requestBody.category },
                { "@questionCount", requestBody.questionsCount },
                { "@userGuid", loggedInUserGuid},
                {"@startDate" , requestBody.startDate },
                {"@sessionDuration",requestBody.sessionDuration},
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
                "WHERE sessions.session_code= @sessionCode";
            var parameters = new Dictionary<string, object> { { "@sessionCode", sessionCode } };
            var sessions = await _dbContext.QueryAsync(sql, reader => new SessionDTO
            {
                sessionCode = reader.GetString(0),
                category = reader.GetString(1),
                userName = reader.GetString(3),
            }, parameters);

            return sessions.FirstOrDefault()!;
        }

        public async Task UpdateSessionAsync(SessionDTO session)
        {
            const string sql = "UPDATE sessions SET name = @name, email = @email WHERE id = @id";

            await _dbContext.ExecuteNonQueryAsync(sql);
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

        public async Task DeleteSessionAsync(string sessionCode)
        {
            const string sql = "Update sessions set end_date=now() WHERE session_code = @sessionCode";
            var parameters = new Dictionary<string, object> { { "@id", sessionCode } };

            await _dbContext.ExecuteNonQueryAsync(sql, parameters);
        }
    }
}