using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GalaxyGuesserCLI.Helpers;
using GalaxyGuesserCLI.Models;

public class SessionScores
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task<ScoreUpdateResponse> UpdatePlayerScores(int sessionId, int points)
    {
        var requestBody = new
        {
            Points = points,
            SessionId = sessionId,
        };
        string jwt = Helper.GetStoredToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync("http://ec2-13-244-67-213.af-south-1.compute.amazonaws.com/api/sessionscore", content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ScoreUpdateResponse>();
        }

        throw new HttpRequestException($"Failed to update scores. Status code: {response.StatusCode}");
    }
}