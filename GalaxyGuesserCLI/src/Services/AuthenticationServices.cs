using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaxyGuesserCLI.Models;
using GalaxyGuesserCLI.Helpers;
using System.Net;
using System.Web;
using System.Security;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;



namespace GalaxyGuesserCLI.Services
{
  public class AuthenticationService
  {
    private static List<Player> players = new List<Player>();
    private readonly IConfiguration _configuration;
    private static readonly HttpClient _httpClient = new HttpClient();

    public AuthenticationService()
    {
      _configuration = _configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .Build();
    }


    internal static List<Player> GetAllPlayers()
    {
      return players;
    }

    internal async Task<Player> AuthOrRegisterWithBackend()
    {
      string jwt = Helper.GetStoredToken();
      var httpClient = new HttpClient();
      httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

      var response = await httpClient.PostAsync("http://ec2-13-244-67-213.af-south-1.compute.amazonaws.com/api/players/auth", new StringContent("null", Encoding.UTF8, "application/json"));
      var result = await Result<Player>.FromApiResponseAsync(response);
      Player player = new Player();
      result.OnSuccess(playerResponse =>
      {
        if (playerResponse.userName == " ")
        {
          Console.Write("\n🌟 Enter your display name: ");
          Console.CursorVisible = true;
          var displayName = Console.ReadLine();
          Console.CursorVisible = false;
        }
        player.userName = playerResponse.userName;
        player.guid = playerResponse.guid;
        player.playerId = playerResponse.playerId;

      })
      .OnFailure(errors =>
      {
        foreach (var error in errors)
        {
          Console.WriteLine($"Authentication failed: {error}");
        }
      });
      Console.WriteLine(player);
      return player!;
    }

    public async Task<string> AuthenticateWithGoogle()
    {
      var listener = new HttpListener();
      listener.Prefixes.Add("http://localhost:5000/");
      listener.Start();

      var state = Guid.NewGuid().ToString();

      var authorizationEndpoint =
          "https://accounts.google.com/o/oauth2/v2/auth" +
          "?client_id=" + "2880504077-78ejpg7rqn6cqr35mjolapla9e232g1b.apps.googleusercontent.com" +
          "&response_type=code" +
          "&scope=openid%20email%20profile" +
          "&redirect_uri=http://localhost:5000/" +
          "&state=" + state;

      Process.Start(new ProcessStartInfo
      {
        FileName = authorizationEndpoint,
        UseShellExecute = true
      });

      Console.WriteLine("A browser window has been opened. Please login with your Google account.");

      var context = await listener.GetContextAsync();

      var query = HttpUtility.ParseQueryString(context.Request.Url.Query);
      var returnedState = query["state"];
      var code = query["code"];

      if (returnedState != state)
      {
        throw new SecurityException("Invalid state parameter");
      }

      var response = context.Response;
      var responseString = "<html><body><h1>Authentication successful!</h1><p>You can close this window now.</p></body></html>";
      var buffer = Encoding.UTF8.GetBytes(responseString);
      response.ContentLength64 = buffer.Length;
      var responseOutput = response.OutputStream;
      await responseOutput.WriteAsync(buffer, 0, buffer.Length);
      responseOutput.Close();

      using var httpClient = new HttpClient();
      var tokenResponse = await httpClient.PostAsync(
      "http://ec2-13-244-67-213.af-south-1.compute.amazonaws.com/api/auth/token",
      new FormUrlEncodedContent(new Dictionary<string, string>
      {
        ["Code"] = code,
        ["RedirectUri"] = "http://localhost:5000/"
      }));


      if (!tokenResponse.IsSuccessStatusCode)
      {
        var errorContent = await tokenResponse.Content.ReadAsStringAsync();
        throw new Exception($"Token request failed: {tokenResponse.StatusCode} - {errorContent}");
      }

      var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
      var tokenData = JsonSerializer.Deserialize<TokenResponse>(tokenContent);

      await StoreTokenInFile(tokenData);

      return tokenData.AccessToken;
    }

    public async Task<ApiResponse<Player>> GetPlayerById(int playerId)
    {
      try
      {
        var url = $"http://ec2-13-244-67-213.af-south-1.compute.amazonaws.com/api/players/{playerId}";
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
          Console.WriteLine($"{await response.Content.ReadAsStringAsync()}");
          return null;
        }
        
        var player = JsonSerializer.Deserialize<ApiResponse<Player>>(responseContent);

        return player;
      }
      catch (Exception ex)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ Unexpected error: {ex.Message}");
        Console.WriteLine(ex.ToString());
        return null;
      }
    }

      public class TokenResponse
    {
      [JsonPropertyName("access_token")]
      public string AccessToken { get; set; }

      [JsonPropertyName("token_type")]
      public string TokenType { get; set; }

      [JsonPropertyName("expires_in")]
      public int ExpiresIn { get; set; }

      [JsonPropertyName("refresh_token")]
      public string RefreshToken { get; set; }
    }

    public class TokenRequest
    {
      public string Code { get; set; }
      public string RedirectUri { get; set; }
    }
    private async Task StoreTokenInFile(TokenResponse tokenData)
    {
      var tokenJson = JsonSerializer.Serialize(tokenData, new JsonSerializerOptions { WriteIndented = true });
      await File.WriteAllTextAsync("token.json", tokenJson);
    }
  }
}