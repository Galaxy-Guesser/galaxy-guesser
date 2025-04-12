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
            const string sql = "SELECT playerId FROM 'players'";
            
            return await _dbContext.QueryAsync(sql, reader => new Player
            {
                player_id = reader.GetInt32(0),
                username = "update me",
                guid = "1"
                // Name = reader.GetString(1),
                // Email = reader.GetString(2),
                // CreatedAt = reader.GetDateTime(3)
            });
        }

        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            const string sql = "SELECT id, name, email, created_at FROM Players WHERE id = @id";
            var parameters = new Dictionary<string, object> { { "@id", id } };
            
            var Players = await _dbContext.QueryAsync(sql, reader => new Player
            {
                player_id = reader.GetInt32(0),
                username = reader.GetString(1),
                guid = reader.GetString(1)
            }, parameters);
            
            return Players.FirstOrDefault()!;
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

            return players.FirstOrDefault(); // 👈 this is the key line!
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

        public async Task UpdatePlayerAsync(Player Player)
        {
            const string sql = "UPDATE Players SET name = @name, email = @email WHERE id = @id";
            var parameters = new Dictionary<string, object>
            {
                { "@id", Player.player_id },
                { "@name", Player.username },
            };
            
            await _dbContext.ExecuteNonQueryAsync(sql, parameters);
        }

        public async Task DeletePlayerAsync(int id)
        {
            const string sql = "DELETE FROM Players WHERE id = @id";
            var parameters = new Dictionary<string, object> { { "@id", id } };
            
            await _dbContext.ExecuteNonQueryAsync(sql, parameters);
        }

        public Task<Player> GetPlayerByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EmailExistsAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}