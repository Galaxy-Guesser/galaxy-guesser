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
                CreatedAt = reader.GetDateTime(3)
            }, parameters);
            
            return Players.FirstOrDefault()!;
        }

        public async Task CreatePlayerAsync(Player Player)
        {
            const string sql = "INSERT INTO Players (name, email, created_at) VALUES (@name, @email, @createdAt)";
            var parameters = new Dictionary<string, object>
            {
                { "@name", Player.username },
                { "@createdAt", DateTime.UtcNow }
            };
            
            await _dbContext.ExecuteNonQueryAsync(sql, parameters);
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