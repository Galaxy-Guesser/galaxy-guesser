using Spectre.Console;
using System.Text;
using System.Net.Http.Headers;
using System.Text.Json;
using ConsoleApp1.Models;
using ConsoleApp1.Helpers;
using ConsoleApp1.Services;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json.Serialization;

namespace ConsoleApp1.Services
{
  public class SessionService
  {
    private static List<Session> sessions = new List<Session>();
    private static List<SessionPlayer> sessionPlayers = new List<SessionPlayer>();
    private static List<SessionQuestion> sessionQuestions = new List<SessionQuestion>();
    private static List<SessionScore> sessionScores = new List<SessionScore>();

    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task<string?> CreateSessionAsync(string category, int questionsCount, string startDate, decimal sessionDuration)
    {
      try
      {
        string jwt = Helper.GetStoredToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        var url = $"http://localhost:5010/api/sessions/session";

        var request = new CreateSessionRequest
        {
          category = category,
          questionsCount = questionsCount,
          startDate = startDate,
          sessionDuration = sessionDuration
        };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        string sessionCode = await response.Content.ReadAsStringAsync();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✅ Session created: {sessionCode}");
        Console.ResetColor();

        return sessionCode;
      }
      catch (Exception ex)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ Error creating session: {ex.Message}");
        Console.ResetColor();
        return null;
      }
    }

    public static async Task JoinSessionAsync(string sessionCode)
    {
      try
      {
        string jwt = Helper.GetStoredToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);

        var playerGuid = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value
                      ?? token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var requestBody = new
        {
          sessionCode = sessionCode,
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");



        var url = "http://ec2-13-244-67-213.af-south-1.compute.amazonaws.com/api/sessions";
        HttpResponseMessage response = await _httpClient.PostAsync(url, content);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
          Console.ForegroundColor = ConsoleColor.Green;
          Console.WriteLine($"✅ Successfully joined session: {sessionCode}");
          var questions = await SessionQuestionViewService.GetAllSessionQuestions(sessionCode);
          await UIService.DisplaySessionQuestionsAsync(questions);
        }
        else
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine($"🔍 Error : {responseContent}");
        }
      }
      catch (Exception ex)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ Unexpected error: {ex.Message}");
      }
      finally
      {
        Console.ResetColor();
      }

    }

    public static async void ChangeUsername(int playerId, string username, string guid)
    {
      try
      {
        var requestBody = new Player
        {
          playerId = playerId,
          userName = username,
          guid = guid
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var url = $"http://ec2-13-244-67-213.af-south-1.compute.amazonaws.com/api/players/{playerId}";
        HttpResponseMessage response = await _httpClient.PutAsync(url, content);
        string responseContent = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        if (!response.IsSuccessStatusCode)
        {
          Console.WriteLine($"{requestBody}");
          Console.WriteLine($"{await response.Content.ReadAsStringAsync()}");
        }
        //var updatedPlayer = await authService.GetPlayerById(player.playerId);
        //player.userName = updatedPlayer.userName;
      }
      catch (Exception ex)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ Unexpected error: {ex.Message} YOUR ERROR HERE");
        Console.WriteLine(ex.ToString());
      }
    }

    public static async Task<List<PlayerStatsDTO>> ViewPlayerStats(int playerId)
    {
      try
      {
        var url = $"http://localhost:5010/api/players/{playerId}/stats";
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        string responseContent = await response.Content.ReadAsStringAsync();
        //response.EnsureSuccessStatusCode();

        if (!response.IsSuccessStatusCode)
        {
          Console.WriteLine($"{await response.Content.ReadAsStringAsync()}");
          return new List<PlayerStatsDTO>();
        }

        var options = new JsonSerializerOptions
        {
          PropertyNameCaseInsensitive = true
        };

        var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<PlayerStatsDTO>>>(responseContent, options);
        var data = apiResponse?.Data ?? new List<PlayerStatsDTO>();

        foreach (var stat in data)
        {
          stat.sessionScore ??= 0;
          stat.ranking ??= 0;
          stat.category ??= "Unknown";
          stat.sessionCode ??= "N/A";
        }
        return data;
      }
      catch (Exception ex)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ Unexpected error: {ex.Message}");
        Console.WriteLine(ex.ToString());
        return new List<PlayerStatsDTO>();
      }
    }
  }
}