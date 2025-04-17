using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class QuestionsService
{
    private static readonly HttpClient _httpClient = new HttpClient();
    public static async Task<int> FetchSessionQuestionsCountAsync(int categoryId)
    {
            var response = await _httpClient.GetAsync($"http://localhost:5010/api/questions?categoryId={categoryId}");
            var questionsCount = await response.Content.ReadFromJsonAsync<int>();
            return questionsCount;

    }
}
