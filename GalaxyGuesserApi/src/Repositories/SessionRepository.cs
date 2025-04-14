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
        public async Task CreateSessionAsync(CreateSessionRequestDTO requestBody)
        {
            const string sql = "CALL create_session (@category,@userGuid,@startDate,@questionCount,@questionDuration)";
            var parameters = new Dictionary<string, object>
             {
                { "@category", requestBody.category },
                { "@questionCount", requestBody.questionsCount },
                { "@userGuid", requestBody.userGuid},
                {"@startDate" , requestBody.startDate },
                {"@questionDuration",requestBody.questionDuration},
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
            var parameters = new Dictionary<string, object>
            {
                // { "@id", Player.player_id },
                // { "@name", Player.username },
            };

            await _dbContext.ExecuteNonQueryAsync(sql, parameters);
        }

        public async Task DeleteSessionAsync(string sessionCode)
        {
            const string sql = "Update sessions set end_date=now() WHERE session_code = @sessionCode";
            var parameters = new Dictionary<string, object> { { "@id", sessionCode } };

            await _dbContext.ExecuteNonQueryAsync(sql, parameters);
        }
    }
}