using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.Text.Json;
using ConsoleApp1.Models;
using ConsoleApp1.Data;
using ConsoleApp1.Helpers;



namespace ConsoleApp1.Services
{
    public class SessionScoreService
    {
       
        private static readonly HttpClient _httpClient = new HttpClient();


        public static async Task<bool> UpdateScoreAsync(int playerId, int sessionId, int points)
        {
            try
            {
                string jwt = Helper.GetStoredToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                var request = new ScoreUpdateRequest
                {
                    PlayerId = playerId,
                    SessionId = sessionId,
                    Points = points 
                };

                var url = "http://ec2-13-244-67-213.af-south-1.compute.amazonaws.com/api/sessionscore";
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var scoreResponse = JsonSerializer.Deserialize<ScoreUpdateResponse>(responseContent);

                if (scoreResponse?.Success == true)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✅ Correct! Youre on {scoreResponse.UpdatedScore} points");
                    return true;
                } else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"❌OOPS, Score update failed: {scoreResponse?.Message ?? "Unknown error"}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Error updating score: {ex.Message}");
                return false;
            }
            finally
            {
                Console.ResetColor();
            }
        }


        public class ScoreUpdateRequest
        {
            public int PlayerId { get; set; }
            public int SessionId { get; set; }
            public int Points { get; set; }
        }

        // Response model (keep only needed properties)
        public class ScoreUpdateResponse
        {
            public bool Success { get; set; }
            public int UpdatedScore { get; set; }
            public string Message { get; set; }
        }

    }

}