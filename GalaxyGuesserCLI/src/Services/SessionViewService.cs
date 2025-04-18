using System.Net.Http.Headers;
using ConsoleApp1.Helpers;
using ConsoleApp1.Models;

namespace ConsoleApp1.Services
{
    public class SessionViewService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        private const string ActiveSessionsApiUrl = "http://ec2-13-244-67-213.af-south-1.compute.amazonaws.com/api/change";
        public static async Task<List<SessionView>> GetActiveSessions()
        {
            try
            {
                string jwt = Helper.GetStoredToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

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