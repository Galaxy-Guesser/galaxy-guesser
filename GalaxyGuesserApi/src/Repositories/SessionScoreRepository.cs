using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaxyGuesserApi.Data;
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
        public async Task<bool> UpdatePlayerScoreAsync(int sessionId, int playerId, int score)
        {
            const string sql = @"
            UPDATE SessionScores
            SET score = @Score
            WHERE session_id = @SessionId AND player_id = @PlayerId;
              ";
            var parameters = new Dictionary<string, object>
            {
              
                {"@playerId", playerId},
                { "@sessionId", sessionId },
                { "@score", score }
            };

            var affectedRows = await _dbContext.ExecuteAsync(sql, parameters);
            return affectedRows > 0;

        }
    }
}