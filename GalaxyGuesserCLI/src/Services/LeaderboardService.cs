
using System.Text.Json;
using static GalaxyGuesserCLI.Models.Leaderbored;

namespace GalaxyGuesserCLI.Services
{
    public static class LeaderboardService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string BaseUrl = "http://localhost:5010/api/leaderboard";

        public static async Task<List<GlobalLeaderboardEntry>> GetGlobalLeaderboardAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/global");
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync();
                
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                var result = JsonSerializer.Deserialize<GlobalLeaderboardResponse>(content, options);
                
                return result?.Leaderboard ?? new List<GlobalLeaderboardEntry>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetGlobalLeaderboardAsync: {ex}");
                return new List<GlobalLeaderboardEntry>();
            }
        }

        public static async Task<List<LeaderboardEntry>> GetSessionLeaderboardAsync(string sessionCode)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/session/{sessionCode}");
                               
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.ReasonPhrase}");
                    return new List<LeaderboardEntry>();
                }

                var content = await response.Content.ReadAsStringAsync();
                
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var result = JsonSerializer.Deserialize<SessionLeaderboardResponse>(content, options);
                
                return result?.Leaderboard ?? new List<LeaderboardEntry>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Service Error: {ex}");
                return new List<LeaderboardEntry>();
            }
        }
    }
}