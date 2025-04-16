using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ConsoleApp1.Helpers;
using ConsoleApp1.Models;

namespace ConsoleApp1.Services
{
    public class SessionQuestionViewService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string SessionQuestionsApiUrl = "http://ec2-13-244-67-213.af-south-1.compute.amazonaws.com/api/sessionQuestions";

        public static async Task<List<SessionQuestionView>> GetAllSessionQuestions(string sessionCode)
        {
            try
            {
                string jwt = Helper.GetStoredToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                string requestUrl = $"{SessionQuestionsApiUrl}?sessionCode={sessionCode}";
                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                var questions = await response.Content.ReadAsAsync<List<SessionQuestionView>>();
                return questions ?? new List<SessionQuestionView>();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Error fetching session questions: {ex.Message}");
                Console.ResetColor();
                return new List<SessionQuestionView>();
            }
        }
    }
}
