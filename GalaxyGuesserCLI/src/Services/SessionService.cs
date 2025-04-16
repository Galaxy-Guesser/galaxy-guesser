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
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;



namespace ConsoleApp1.Services
{
  public class SessionService
  {
    // In-memory data storage (could be moved to a repository pattern in future)
    private static List<Session> sessions = new List<Session>();
    private static List<SessionPlayer> sessionPlayers = new List<SessionPlayer>();
    private static List<SessionQuestion> sessionQuestions = new List<SessionQuestion>();
    private static List<SessionScore> sessionScores = new List<SessionScore>();



    public static string GenerateSessionCode()
    {
      // Generate a unique 6-character code
      const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
      Random random = new Random();
      string code;

      do
      {
        code = new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[random.Next(s.Length)]).ToArray());
      } while (sessions.Any(s => s.Code == code));

      return code;
    }

    internal static Session CreateSession(Player player, int categoryId, int questionCount, int questionDuration)
    {
      string sessionCode = GenerateSessionCode();
      int sessionId = sessions.Count > 0 ? sessions.Max(s => s.Id) + 1 : 1;

      Session session = new Session(sessionId, sessionCode, categoryId, questionDuration, questionCount);
      sessions.Add(session);

      // Add questions to session
      AddQuestionsToSession(session.Id, categoryId, questionCount);

      // Add player to session
      AddPlayerToSession(player.playerId, session.Id);

      return session;
    }

    internal static Session JoinSession(Player player, string sessionCode)
    {
      Session session = sessions.FirstOrDefault(s => s.Code == sessionCode.ToUpper());

      if (session != null)
      {
        // Add player to session
        AddPlayerToSession(player.playerId, session.Id);
        return session;
      }

      return null; // Session not found
    }

    public static void AddPlayerToSession(int playerId, int sessionId)
    {
      // Check if player already in session
      if (!sessionPlayers.Any(sp => sp.PlayerId == playerId && sp.SessionId == sessionId))
      {
        sessionPlayers.Add(new SessionPlayer(sessionId, playerId));
      }
    }

    public static void AddQuestionsToSession(int sessionId, int categoryId, int questionCount)
    {
      // Get questions for this category and randomize them
      var categoryQuestions = SampleData.Questions
          .Where(q => q.CategoryId == categoryId)
          .OrderBy(q => Guid.NewGuid()) // Random order
          .Take(questionCount)  // Only take requested number of questions
          .ToList();

      int id = sessionQuestions.Count > 0 ? sessionQuestions.Max(sq => sq.Id) + 1 : 1;
      foreach (var question in categoryQuestions)
      {
        sessionQuestions.Add(new SessionQuestion(id++, sessionId, question.Id));
      }
    }

    internal static List<Question> GetSessionQuestions(int sessionId)
    {
      return sessionQuestions
          .Where(sq => sq.SessionId == sessionId)
          .Join(
              SampleData.Questions,
              sq => sq.QuestionId,
              q => q.Id,
              (sq, q) => q
          )
          .ToList();
    }

    public static void SaveScore(int playerId, int sessionId, int score, int timeRemaining = 0)
    {
      sessionScores.Add(new SessionScore(playerId, sessionId, score, timeRemaining));
    }

    public static SessionScore GetPlayerScore(int playerId, int sessionId)
    {
      return sessionScores.FirstOrDefault(s => s.PlayerId == playerId && s.SessionId == sessionId);
    }

    internal static List<dynamic> GetSessionLeaderboard(int sessionId, List<Player> players)
    {
      return sessionScores
          .Where(s => s.SessionId == sessionId)
          .OrderByDescending(s => s.Score + s.TimeRemaining)
          .Select(s => new
          {
            Name = players.First(p => p.playerId == s.PlayerId).userName,
            Score = s.Score,
            TimeBonus = s.TimeRemaining,
            Total = s.Score + s.TimeRemaining
          })
          .ToList<dynamic>();
    }

    internal static async Task<(bool answered, int selectedOption)> WaitForAnswerWithTimeout(Question question, int timeoutSeconds)
    {
      var answerTask = Task.Run(() =>
      {
        ConsoleKeyInfo key;
        int selectedOption;
        do
        {
          key = Console.ReadKey(true);
          selectedOption = char.ToUpper(key.KeyChar) - 'A';
        } while (selectedOption < 0 || selectedOption >= question.Options.Length);

        return selectedOption;
      });

      var delayTask = Task.Delay(timeoutSeconds * 1000);
      var completedTask = await Task.WhenAny(answerTask, delayTask);

      if (completedTask == answerTask)
      {
        return (true, await answerTask);
      }
      else
      {
        return (false, -1);
      }
    }



    public static int GetSessionQuestionsCount(int sessionId)
    {
      return sessionQuestions.Count(sq => sq.SessionId == sessionId);
    }


    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task<string?> CreateSessionAsync(string category, int questionsCount, string startDate, int questionDuration)
    {
      try
      {
        string jwt = Helper.GetStoredToken();
         _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        var url = $"http://ec2-13-244-67-213.af-south-1.compute.amazonaws.com/api/sessions/session";
        //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        //var url = $"http://localhost:5010/api/sessions/session";

        var request = new CreateSessionRequest
        {
          category = category,
          questionsCount = questionsCount,
          startDate = startDate,
          questionDuration = questionDuration
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

        // Try to get sub  or nameidentifier
        var playerGuid = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value
                    ?? token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var requestBody = new
        {
          sessionCode = sessionCode,
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");


        var url = "http://ec2-13-244-67-213.af-south-1.compute.amazonaws.com/api/sessions"; 

        //var url = "http://localhost:5010/api/sessions";
        HttpResponseMessage response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        string responseContent = await response.Content.ReadAsStringAsync();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✅ Successfully joined session: {sessionCode}");
        Console.ResetColor();

        var session = await SessionViewService.GetSessionByCode(sessionCode); // you need to implement this
        if (session == null)
        {
          AnsiConsole.MarkupLine("[red]❌ Failed to retrieve session info.[/]");
          break;
        }

        await SessionService.PlayGameAsync(session.SessionId);
      }
      catch (Exception ex)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ Error joining session: {ex.Message}");
        Console.ResetColor();
      }
    }

    public static async Task PlayGameAsync(int sessionId)
    {
      if (sessionId <= 0)
      {
        AnsiConsole.MarkupLine("[red]❌ Invalid session ID.[/]");
        return;
      }

      int score = 0;
      int totalQuestions = 0;

      while (true)
      {
        var questions = await SessionQuestionService.GetQuestionsAsync(sessionId);
        if (questions == null || !questions.Any())
        {
          AnsiConsole.MarkupLine("[red]❌ No more questions available.[/]");
          break;
        }

        totalQuestions++;
        var question = questions[0]; // Single question from API

        Console.Clear();
        UIService.PrintGalaxyHeader();
        AnsiConsole.MarkupLine("[bold cyan]🎮 Welcome to Galaxy Guesser![/]");
        AnsiConsole.WriteLine("-----------------------------------");

        // Display question using Spectre.Console
        AnsiConsole.MarkupLine($"\n[bold]❓ Question:[/] {question.Question}\n");

        var options = question.Options;
        if (options == null || !options.Any())
        {
          AnsiConsole.MarkupLine("[yellow]⚠️ No options available for this question.[/]");
          continue;
        }

        // Display options as a numbered list
        for (int i = 0; i < options.Count; i++)
        {
          AnsiConsole.WriteLine($"{i + 1}. {options[i].Text}");
        }

        // Prompt for user input
        string input = AnsiConsole.Ask<string>("\n[bold]👉 Your answer (enter number):[/] ");

        if (!int.TryParse(input, out int selectedIndex) || selectedIndex < 1 || selectedIndex > options.Count)
        {
          AnsiConsole.MarkupLine("[yellow]⚠️ Invalid choice.[/]");
          AnsiConsole.MarkupLine("\nPress any key to continue...");
          Console.ReadKey();
          continue;
        }

        var selected = options[selectedIndex - 1];

        if (selected.IsCorrect)
        {
          score++;
          AnsiConsole.MarkupLine("[green]✅ Correct![/]");
        }
        else
        {
          var correct = options.FirstOrDefault(o => o.IsCorrect);
          AnsiConsole.MarkupLine($"[red]❌ Wrong! Correct answer:[/] {(correct != null ? correct.Text : "Not available")}[/]");
        }

        AnsiConsole.MarkupLine($"\n[bold]Score:[/] {score}/{totalQuestions}");
        AnsiConsole.MarkupLine("\nPress any key for the next question (or 'q' to quit)...");
        var key = Console.ReadKey().KeyChar;
        if (key == 'q')
          break;
      }

      // Game over
      Console.Clear();
      UIService.PrintGalaxyHeader();
      AnsiConsole.MarkupLine("[bold cyan]🎉 Game Over![/]");
      AnsiConsole.MarkupLine($"[bold]Final Score:[/] {score}/{totalQuestions} ({score * 100.0 / totalQuestions:F1}%)");

      // Replay option
      if (AnsiConsole.Confirm("\nPlay again?"))
      {
        await PlayGameAsync(sessionId);
      }
    }

  }
}