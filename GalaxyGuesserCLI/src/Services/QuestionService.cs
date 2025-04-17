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
            var response = await _httpClient.GetAsync($"http://ec2-13-244-67-213.af-south-1.compute.amazonaws.com/api/questions?categoryId={categoryId}");
            var questionsCount = await response.Content.ReadFromJsonAsync<int>();
            return questionsCount;

    }
}
