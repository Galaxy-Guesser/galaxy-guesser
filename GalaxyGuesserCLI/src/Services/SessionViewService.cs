using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ConsoleApp1.Models;

namespace ConsoleApp1.Services
{
    public class SessionViewService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        

        private const string ActiveSessionsApiUrl = "http://localhost:5010/api/change"; 
        public static async Task<List<SessionView>> GetActiveSessions()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(ActiveSessionsApiUrl);
                response.EnsureSuccessStatusCode();
                
                var activeSessions = await response.Content.ReadAsAsync<List<SessionView>>();
                
                return activeSessions ?? new List<SessionView>();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Error fetching active sessions: {ex.Message}");
                Console.ResetColor();
                return new List<SessionView>(); 
            }
        }
    }
}