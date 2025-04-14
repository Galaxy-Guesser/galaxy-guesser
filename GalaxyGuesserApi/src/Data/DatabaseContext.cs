using Npgsql;

namespace GalaxyGuesserApi.Data
{
    public class DatabaseContext
    {
        private readonly string? _connectionString = null;

        public DatabaseContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            
            if (string.IsNullOrEmpty(_connectionString))
            {
                var host = "localhost";
                var port = "5433";
                var database = "galaxyGuesser";
                var Username = "postgres";
                var password = "12345";

                _connectionString = $"Host={host};Port={port};Database={database};Username={Username};Password={password};";
            }
        }

        public async Task<NpgsqlConnection> CreateConnectionAsync()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task ExecuteNonQueryAsync(string sql, Dictionary<string, object>? parameters = null)
        {
            using var connection = await CreateConnectionAsync();
            using var command = new NpgsqlCommand(sql, connection);
            
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
            }
            
            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<T>> QueryAsync<T>(string sql, Func<NpgsqlDataReader, T> map, Dictionary<string, object>? parameters = null)
        {
            using var connection = await CreateConnectionAsync();
            using var command = new NpgsqlCommand(sql, connection);
            
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
            }
            
            using var reader = await command.ExecuteReaderAsync();
            var results = new List<T>();
            
            while (await reader.ReadAsync())
            {
                results.Add(map(reader));
            }
            
            return results;
        }
        public async Task<int> ExecuteAsync(string sql, Dictionary<string, object>? parameters = null)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
            }

            return await cmd.ExecuteNonQueryAsync();
        }
    }
}