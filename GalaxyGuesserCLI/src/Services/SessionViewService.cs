using System.Net.Http.Headers;
using GalaxyGuesserCLI.Helpers;
using GalaxyGuesserCLI.DTO;

namespace GalaxyGuesserCLI.Services
{
     public class SessionViewService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string SessionsApiUrl = "http://ec2-13-244-67-213.af-south-1.compute.amazonaws.com/api/sessions";

        public static async Task<List<SessionView>> GetSessions(bool getActive = false)
        {
            try
            {
                string jwt = Helper.GetStoredToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                string requestUrl = $"{SessionsApiUrl}?getActive={getActive.ToString().ToLower()}";
                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                var sessions = await response.Content.ReadAsAsync<List<SessionView>>();
                return sessions ?? new List<SessionView>();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Error fetching sessions: {ex.Message}");
                Console.ResetColor();
                return new List<SessionView>();
            }
        }

        public static async Task<List<SessionView>> GetActiveSessions()
        {
            return await GetSessions(true);
        }

        public static async Task<List<SessionView>> GetAllSessions()
        {
            return await GetSessions(false);
        }

    }
}