
using System.Net.Http.Headers;
using System.Text.Json;
using ConsoleApp1.Helpers;
using static GalaxyGuesserCLI.Models.Leaderbored;

namespace GalaxyGuesserCLI.Services
{
    public static class LeaderboardService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string BaseUrl = "http://ec2-13-244-67-213.af-south-1.compute.amazonaws.com/api/leaderboard";


        public static async Task<List<GlobalLeaderboardEntry>> GetGlobalLeaderboardAsync()
        {
            try
            {
                string jwt = Helper.GetStoredToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                var response = await _httpClient.GetAsync($"{BaseUrl}/global");
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                
                var responseObj = JsonSerializer.Deserialize<GlobalLeaderboardResponse>(
                    responseBody,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return responseObj?.Leaderboard ?? new List<GlobalLeaderboardEntry>();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Error: {ex.Message}");
                if (ex is JsonException jsonEx)
                {
                    Console.WriteLine($"JSON Path: {jsonEx.Path} | Line: {jsonEx.LineNumber}");
                }
                Console.ResetColor();
                return new List<GlobalLeaderboardEntry>();
            }
        }

        public static async Task<List<LeaderboardEntry>> GetSessionLeaderboardAsync(string sessionCode)
        {
             try
            {
                string jwt = Helper.GetStoredToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                var response = await _httpClient.GetAsync($"{BaseUrl}/session/{sessionCode}");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<SessionLeaderboardResponse>(responseBody,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result?.Leaderboard ?? new List<LeaderboardEntry>();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Error retrieving session leaderboard: {ex.Message}");
                Console.ResetColor();
                return new List<LeaderboardEntry>();
            }
        }
    }
}