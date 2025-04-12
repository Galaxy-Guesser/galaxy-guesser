using GalaxyGuesserApi.Data;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;

namespace GalaxyGuesserApi.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly DatabaseContext _dbContext;

        public PlayerRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            const string query = @"
            SELECT player_id, username, guid
            FROM player";

            return await _dbContext.QueryAsync(query, reader => new Player
            {
                player_id = reader.GetInt32(0),
                username = reader.GetString(1),
                guid = reader.GetString(2)
            });
            }

        public async Task<Player> GetPlayerByIdAsync(int player_id)
        {
            const string query = @"
            SELECT player_id, username, guid
            FROM player
            WHERE player_id = @player_id";

            var parameters = new Dictionary<string, object> { { "@player_id", player_id } };

            var result = await _dbContext.QueryAsync(query, reader => new Player
                {
                    player_id = reader.GetInt32(0),
                    username = reader.GetString(1),
                    guid = reader.GetString(2)
                }, parameters);

            return result.FirstOrDefault()!;
        }

        public async Task<Player?> GetUserByGoogleIdAsync(string guid)
        {
            const string query = @"
                SELECT player_id, username, guid
                FROM player
                WHERE guid = @guid";

            var parameters = new Dictionary<string, object> { { "@guid", guid } };

            var players = await _dbContext.QueryAsync(query, reader => new Player
            {
                player_id = reader.GetInt32(0),
                username = reader.GetString(1),
                guid = reader.GetString(2)
            }, parameters);

            return players.FirstOrDefault(); 
        }

        public async Task<Player> CreatePlayerAsync(string guid, string username)
        {
            const string sql = @"
            INSERT INTO player (username, guid)
            VALUES (@username, @guid)
            RETURNING player_id, username, guid";
            var parameters = new Dictionary<string, object>
            {
                { "@username", username },
                { "@guid", guid}
            };
            
            var result = await _dbContext.QueryAsync(sql, reader => new Player
            {
                player_id = reader.GetInt32(0),    
                username = reader.GetString(1),    
                guid = reader.GetString(2)       
            }, parameters);

            return result.First();
           
        }

        public async Task<bool> UpdatePlayerAsync(int player_id, string username)
        {
            const string query = @"
            UPDATE player
            SET username = @username
            WHERE player_id = @player_id";

            var parameters = new Dictionary<string, object>
            {
                { "@player_id", player_id },
                { "@username", username }
            };

            var affectedRows = await _dbContext.ExecuteAsync(query, parameters);
            return affectedRows > 0;
        }

        public async Task<bool> DeletePlayerAsync(int player_id)
        {
            const string query = @"
            DELETE FROM player
            WHERE player_id = @player_id";

            var parameters = new Dictionary<string, object> { { "@player_id", player_id } };

            var affectedRows = await _dbContext.ExecuteAsync(query, parameters);
            return affectedRows > 0;
        }
    }
}