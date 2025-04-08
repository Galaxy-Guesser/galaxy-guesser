using Npgsql;

namespace GalaxyGuesserApi.Data
{
    public class DatabaseContext
    {
        private readonly string? _connectionString = null;

        public DatabaseContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresConnection");
            
            if (string.IsNullOrEmpty(_connectionString))
            {
                var host = configuration["DB_HOST"] ?? "localhost";
                var port = configuration["DB_PORT"] ?? "5432";
                var database = configuration["DB_NAME"] ?? "testGuesser";
                var Username = configuration["DB_USER"] ?? "postgres";
                var password = configuration["DB_PASSWORD"] ?? "";

                _connectionString = $"Host={host};Port={port};Database={database};Playername={Username};Password={password};";
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
    }
}