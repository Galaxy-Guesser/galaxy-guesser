using Spectre.Console;
using System;
using System.Collections.Generic;
using ConsoleApp1.Models;
using System.Text;
using GalaxyGuesserCLI.Services;

namespace ConsoleApp1.Services
{
    public static class UIService
    {
        private const string CMD_PREFIX = "/";
        private static readonly (string Key, string Description)[] COMMANDS = new[]
        {
            ("help", "Show help information"),
            ("howtoplay", "Show how to play instructions"),
            ("categories", "List available categories"),
            ("sessions", "List or join quiz sessions"),
            ("quit", "Exit the application")
        };

        public static void ShowHowToPlay()
        {
            Console.Clear();
            PrintGalaxyHeader();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n🚀 HOW TO PLAY GALAXY QUIZ");
            Console.ResetColor();

            Console.WriteLine("\n1️⃣ CREATE OR JOIN A SESSION");
            Console.WriteLine("   • Create a new quiz session by selecting a category, question count, and time limit");
            Console.WriteLine("   • Or join an existing session by entering the session code provided by another player");

            Console.WriteLine("\n2️⃣ ANSWER THE QUESTIONS");
            Console.WriteLine("   • Each question has multiple-choice answers (A, B, C, D)");
            Console.WriteLine("   • Press the letter key corresponding to your answer");
            Console.WriteLine("   • Answer correctly to earn points");
            Console.WriteLine("   • Answer quickly to earn time bonus points (quicker = more bonus points)");

            Console.WriteLine("\n3️⃣ SCORING SYSTEM");
            Console.WriteLine("   • 1 point for each correct answer");
            Console.WriteLine("   • Bonus points for remaining time (1 point per second)");
            Console.WriteLine("   • Final score = correct answers + time bonus");

            Console.WriteLine("\n4️⃣ LEADERBOARDS");
            Console.WriteLine("   • Compare your scores with other players");
            Console.WriteLine("   • View your statistics and session history");

            Console.WriteLine("\n5️⃣ COMMANDS");
            Console.WriteLine("   • Use commands like '/help', '/categories', '/sessions' anytime");
            Console.WriteLine("   • Type the command starting with '/' at any input prompt");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n🌟 TIPS 🌟");
            Console.ResetColor();
            Console.WriteLine("• The faster you answer correctly, the more points you earn");
            Console.WriteLine("• Study the different categories to improve your knowledge");
            Console.WriteLine("• Create sessions with friends and compete for high scores");
        }

       
public static void PrintGalaxyHeader()
{
    Console.Clear();

    string[] galaxyTitle = new string[]
    {
        @"   ___________________________________________________________",
        @"  /    _____       _             _   _        _               \",
        @" |    / ____|     | |           | | (_)      | |               |",
        @" |   | |  __  __ _| | __ ___  __| |_ _  ___  | |__             |",
        @" |   | | |_ |/ _` | |/ _` \ \/ /| __| |/ __| | '_ \            |",
        @" |   | |__| | (_| | | (_| |>  < | |_| | (__  | | | |           |",
        @" |    \_____|\__,_|_|\__,_/_/\_\ \__|_|\___| |_| |_|           |",
        @"  \___________________________________________________________/",
    };

    // Display main title with color gradient
    DisplayColorGradient(galaxyTitle, ConsoleColor.DarkMagenta, ConsoleColor.Cyan);
    
    // Subtitle with pulsing effect
    string[] subtitle = new string[]
    {
        @"     ☄️  EXPLORE THE UNIVERSE - DISCOVER NEW WORLDS  ☄️"
    };
    
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine();
    foreach (string line in subtitle)
    {
        Console.WriteLine(line);
    }
    
    // Elegant separator
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine("\n⭐ ════════════════════════════════════════════════════ ⭐");
    Console.ResetColor();
    Console.WriteLine();
}

// Displays text with a color gradient effect
private static void DisplayColorGradient(string[] text, ConsoleColor startColor, ConsoleColor endColor)
{
    // Preserve original colors
    ConsoleColor originalFg = Console.ForegroundColor;
    
    // Define colors for gradient (can be expanded)
    ConsoleColor[] gradientColors = new ConsoleColor[]
    {
        startColor,
        ConsoleColor.Magenta,
        ConsoleColor.Blue,
        ConsoleColor.Cyan,
        endColor
    };
    
    // Display each line with appropriate color
    for (int i = 0; i < text.Length; i++)
    {
        // Calculate which color to use based on position
        int colorIndex = (int)Math.Floor((double)i / text.Length * gradientColors.Length);
        if (colorIndex >= gradientColors.Length) colorIndex = gradientColors.Length - 1;
        
        Console.ForegroundColor = gradientColors[colorIndex];
        Console.WriteLine(text[i]);
    }
    
    // Reset color
    Console.ForegroundColor = originalFg;
}

        public static void ShowHelp(Dictionary<string,string> COMMANDS)
        {
            Console.Clear();
            PrintGalaxyHeader();
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n📖 AVAILABLE COMMANDS");
            Console.ResetColor();
            
            Console.WriteLine($"\nUse {CMD_PREFIX}[command] to execute any of these commands:");
            
            foreach (var cmd in COMMANDS)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{CMD_PREFIX}{cmd.Key,-15}");
                Console.ResetColor();
                Console.WriteLine($" - {cmd.Value}");
            }
            
            Console.WriteLine("\nYou can use commands at any input prompt in the application.");
        }

        public static void ShowFeedback(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine("\n" + message);
            Console.ResetColor();
        }

        public static void Continue()
    {
        Console.WriteLine("\nReturn to main menu...");
        Console.ReadKey(true);
    }

     static void DisplayFullQuestion(Question q, int current, int total, int secondsRemaining)
        {
            Console.Clear();
            PrintGalaxyHeader();
            
            Console.WriteLine($"⏱ Time: {secondsRemaining}s [" + new string(' ', Console.WindowWidth - 20) + "]");
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nQuestion {current}/{total}:");
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n{q.Text}\n");
            Console.ResetColor();
            Console.WriteLine(); // Extra spacing
            Console.WriteLine("Answer options:");
            for (int i = 0; i < q.Options.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{(char)('A' + i)}) ");
                Console.ResetColor();
                Console.WriteLine(q.Options[i]);
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("\n👉 Press A, B, C or D to select your answer: ");
            Console.ResetColor();
            
            // Make answer input area very visible
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("     ");
            Console.ResetColor();
        }

         internal static void ShowFinalResults(Player player, Session session)
        {
            Console.Clear();
            PrintGalaxyHeader();

            SessionScore playerScore = SessionService.GetPlayerScore(player.playerId, session.Id);
            int score = playerScore != null ? playerScore.Score : 0;
            int timeBonus = playerScore != null ? playerScore.TimeRemaining : 0;

            int totalQuestions = SessionService.GetSessionQuestionsCount(session.Id);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n🌟 Final Score: {score}/{totalQuestions} correct answers");
            Console.WriteLine($"⏱ Time Bonus: {timeBonus} points");
            Console.WriteLine($"🏆 Total Score: {score + timeBonus} points\n");

            // Get leaderboard
            Console.WriteLine("🏆 Leaderboard:");
            var leaderboard = SessionService.GetSessionLeaderboard(session.Id, AuthenticationService.GetAllPlayers());
            
            for (int i = 0; i < leaderboard.Count; i++)
            {
                var entry = leaderboard[i];
                Console.WriteLine($"{i+1}. {entry.Name}: {entry.Score} correct + {entry.TimeBonus} time bonus = {entry.Total} points");
            }

            Console.ResetColor();
        }

        internal static void UpdateTimerOnly(int row, int secondsRemaining, int totalSeconds)
        {
            int originalRow = Console.CursorTop;
            int originalCol = Console.CursorLeft;
            
            // Move to timer position
            Console.SetCursorPosition(0, row);
            
            // Calculate bar width
            int barWidth = Console.WindowWidth - 20;
            int filledWidth = (int)((double)secondsRemaining / totalSeconds * barWidth);
            
            // Update timer with appropriate color
            Console.ForegroundColor = secondsRemaining > 10 ? ConsoleColor.Green : 
                                    secondsRemaining > 5 ? ConsoleColor.Yellow : ConsoleColor.Red;
            Console.Write($"⏱ Time: {secondsRemaining}s [");
            Console.Write(new string('■', filledWidth));
            Console.Write(new string('□', barWidth - filledWidth));
            Console.Write("]");
            Console.ResetColor();
            
            // Move cursor back to original position
            Console.SetCursorPosition(originalCol, originalRow);
        }

        public static async Task DisplayActiveSessionsAsync(List<SessionView> sessions)
        {
            AnsiConsole.MarkupLine("\n📡 [bold underline]Active Sessions[/]");

            if (sessions.Count == 0)
            {
                AnsiConsole.MarkupLine("[grey]No active sessions found.[/]");
                return;
            }

            var grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();

            var panels = sessions.Select(session =>
            {
                var timeParts = session.endsIn.Split(new[] { 'm', 's' }, StringSplitOptions.RemoveEmptyEntries);
                int minutes = int.TryParse(timeParts[0], out var parsedMinutes) ? parsedMinutes : 0;

                var color = minutes < 5 ? "red" : "green";

                return new Panel(new Markup(
                    $"[bold]{session.category}[/]\n" +
                    $"[blue]Code:[/] {session.sessionCode}\n" +
                    $"[blue]Ends In:[/] [{color}]{minutes}m[/]"))
                {
                    Border = BoxBorder.Double,
                    Padding = new Padding(1, 0, 1, 0)
                };
            }).ToList();

            for (int i = 0; i < panels.Count; i += 2)
            {
                if (i + 1 < panels.Count)
                    grid.AddRow(panels[i], panels[i + 1]);
                else
                    grid.AddRow(panels[i]);
            }

            AnsiConsole.Write(grid);

            var sessionCode = AnsiConsole.Ask<string>("\n▶️ [bold yellow]Enter a session code to join or press Enter to cancel:[/]");

            if (!string.IsNullOrWhiteSpace(sessionCode))
            {
                var selectedSession = sessions.FirstOrDefault(s => s.sessionCode.Equals(sessionCode, StringComparison.OrdinalIgnoreCase));
                if (selectedSession != null)
                {
                    await SessionService.JoinSessionAsync(sessionCode);
                }
                else
                {
                    AnsiConsole.MarkupLine($"❌ [red]Invalid session code:[/] {sessionCode}");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[grey]No session joined.[/]");
            }
        }

        public static void DisplaySpaceFact(string fact)
        {
            Console.Clear();
            PrintGalaxyHeader();
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("🌟 Space Fact of the Day 🌟");
            Console.WriteLine(new string('─', Console.WindowWidth));
            Console.ResetColor();
            
            var words = fact.Split(' ');
            var line = new StringBuilder();
            foreach (var word in words)
            {
                if (line.Length + word.Length > Console.WindowWidth - 4)
                {
                    Console.WriteLine($"  {line}");
                    line.Clear();
                }
                line.Append(word + " ");
            }
            Console.WriteLine($"  {line}");
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nCourtesy of NASA's Astronomy Picture of the Day");
            Console.ResetColor();
            
            Continue();
        }

public static async Task DisplaySessionQuestionsAsync(List<SessionQuestionView> questions)
{
    if (questions.Count == 0)
    {
        AnsiConsole.MarkupLine("[grey]No questions found for this session.[/]");
        return;
    }

    foreach (var question in questions)
    {
        AnsiConsole.Clear();

        // Display question + options in card
        var questionPanel = new Panel(BuildQuestionMarkup(question))
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(1, 0, 1, 0),
            Header = new PanelHeader($"[bold green]Question {question.questionId}[/]", Justify.Center)
        };

        AnsiConsole.Write(questionPanel);

        var prompt = new SelectionPrompt<string>()
            .Title("\n[bold]Select your answer:[/]")
            .HighlightStyle("cyan");

        var optionMap = new Dictionary<string, Option>();

        for (int i = 0; i < question.options.Count; i++)
        {
            var label = $"{i + 1}. {question.options[i].optionText}";
            prompt.AddChoice(label);
            optionMap[label] = question.options[i];
        }

        var selectedLabel = AnsiConsole.Prompt(prompt);
        var selectedOption = optionMap[selectedLabel];

        bool isCorrect = selectedOption.answerId == question.correctAnswerId;
        var resultColor = isCorrect ? "green" : "red";
        var resultText = isCorrect ? "✔ Correct!" : "✘ Incorrect.";

        AnsiConsole.MarkupLine($"\n[{resultColor}]{resultText}[/]");
        await Task.Delay(2000);
    }

    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold green]✅ All questions completed.[/]");
}



private static string BuildQuestionMarkup(SessionQuestionView question)
{
    var markup = $"[bold underline]{question.questionText}[/]\n\n";

    for (int i = 0; i < question.options.Count; i++)
    {
        markup += $"[blue]{i + 1}.[/] {question.options[i].optionText}\n";
    }

    return markup;
}

/// <summary>
/// Displays a loading indicator with the specified message
/// </summary>
/// <param name="message">Message to display while loading</param>
public static void ShowLoader(string message)
{
    int originalTop = Console.CursorTop;
    int originalLeft = Console.CursorLeft;
    
    // Save current console colors
    ConsoleColor originalFg = Console.ForegroundColor;
    ConsoleColor originalBg = Console.BackgroundColor;
    
    // Create loading indicator line
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write($"\r🔄 {message}");
    
    // Store the position info in static variables so HideLoader can access them
    loaderActive = true;
    loaderPosition = (Console.CursorLeft, Console.CursorTop);
    loaderMessage = message;
    
    // Reset colors
    Console.ForegroundColor = originalFg;
    Console.BackgroundColor = originalBg;
    
    // Start animation task if not already running
    if (loaderTask == null || loaderTask.IsCompleted)
    {
        loaderTask = StartLoaderAnimationAsync();
    }
}

/// <summary>
/// Hides the active loading indicator
/// </summary>
public static void HideLoader()
{
    // Stop the animation
    loaderActive = false;
    
    // Clear the loader line if it was displayed
    if (loaderPosition.HasValue)
    {
        int currentTop = Console.CursorTop;
        int currentLeft = Console.CursorLeft;
        
        Console.SetCursorPosition(0, loaderPosition.Value.Top);
        Console.Write(new string(' ', loaderMessage.Length + 10)); // Clear the line with spaces
        
        // Reset cursor position
        Console.SetCursorPosition(currentLeft, currentTop);
        
        // Reset loader state
        loaderPosition = null;
        loaderMessage = string.Empty;
    }
}

// Add these fields to the UIService class
private static bool loaderActive = false;
private static (int Left, int Top)? loaderPosition = null;
private static string loaderMessage = string.Empty;
private static Task loaderTask = null;

/// <summary>
/// Handles the loader animation
/// </summary>
private static async Task StartLoaderAnimationAsync()
{
    string[] animationFrames = new[] { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" };
    int frameIndex = 0;
    
    while (loaderActive)
    {
        if (loaderPosition.HasValue)
        {
            int currentTop = Console.CursorTop;
            int currentLeft = Console.CursorLeft;
            
            Console.SetCursorPosition(0, loaderPosition.Value.Top);
            
            ConsoleColor originalFg = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            
            Console.Write($"\r{animationFrames[frameIndex]} {loaderMessage}");
            
            Console.ForegroundColor = originalFg;
            Console.SetCursorPosition(currentLeft, currentTop);
            
            frameIndex = (frameIndex + 1) % animationFrames.Length;
        }
        
        await Task.Delay(100); // Update animation every 100ms
    }
}

        public static async Task DisplayGlobalLeaderboard()
        {
            try
            {
                AnsiConsole.Status()
                    .Start("Loading global leaderboard...", ctx => 
                    {
                        ctx.Spinner(Spinner.Known.Star);
                        ctx.SpinnerStyle(Style.Parse("green"));
                    });

                var leaderboard = await LeaderboardService.GetGlobalLeaderboardAsync();
                
                AnsiConsole.MarkupLine("[yellow]DEBUG:[/] Received leaderboard entries: " + leaderboard.Count);
                foreach (var entry in leaderboard.Take(3))
                {
                    AnsiConsole.MarkupLine($"[yellow]DEBUG:[/] {entry.Rank}. {entry.UserName} - {entry.TotalScore}");
                }

                AnsiConsole.Clear();
                UIService.PrintGalaxyHeader();
                AnsiConsole.MarkupLine("\n[bold yellow]🌌 GLOBAL LEADERBOARD[/]\n");

                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Purple)
                    .AddColumn(new TableColumn("[bold]Rank[/]").Centered())
                    .AddColumn(new TableColumn("[bold]Player[/]").Centered())
                    .AddColumn(new TableColumn("[bold]Total Score[/]").Centered())
                    .AddColumn(new TableColumn("[bold]Sessions[/]").Centered());

                if (leaderboard.Count == 0)
                {
                    table.AddRow("[red]No data available[/]", "", "", "", "");
                }
                else
                {
                    foreach (var entry in leaderboard)
                    {
                        var sessionsList = entry.Sessions?.Any() == true 
                            ? string.Join("\n", entry.Sessions.Take(3)) 
                            : "None";
                        
                        if (entry.Sessions?.Count > 3)
                        {
                            sessionsList += $"\n...and {entry.Sessions.Count - 3} more";
                        }

                        table.AddRow(
                            $"[bold]#{entry.Rank}[/]",
                            Markup.Escape(entry.UserName),
                            entry.TotalScore.ToString(),
                            sessionsList
                        );
                    }
                }

                AnsiConsole.Write(table);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine("[red]Error loading leaderboard:[/] " + ex.Message);
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
            }
        }

        public static async Task DisplaySessionLeaderboard()
        {
            try
            {
                var sessionCode = AnsiConsole.Ask<string>("Enter session code:");
     
                AnsiConsole.Status()
                    .Start("Loading session leaderboard...", ctx => 
                    {
                        ctx.Spinner(Spinner.Known.Star);
                        ctx.SpinnerStyle(Style.Parse("blue"));
                    });

                var leaderboard = await LeaderboardService.GetSessionLeaderboardAsync(sessionCode);

                if (leaderboard != null)
                {
                    foreach (var entry in leaderboard.Take(3))
                    {
                        Console.WriteLine($"DEBUG: {entry.Rank}. {entry.UserName} - {entry.Score}");
                    }
                }

                AnsiConsole.Clear();
                UIService.PrintGalaxyHeader();
                AnsiConsole.MarkupLine($"\n[bold yellow]🚀 SESSION LEADERBOARD: {sessionCode}[/]\n");

                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Blue)
                    .AddColumn(new TableColumn("[bold]Rank[/]").Centered())
                    .AddColumn(new TableColumn("[bold]Player[/]").Centered())
                    .AddColumn(new TableColumn("[bold]Score[/]").Centered());

                if (leaderboard == null || leaderboard.Count == 0)
                {
                    table.AddRow("[red]No data available[/]", "Try a different session code", "");
                }
                else
                {
                    foreach (var entry in leaderboard)
                    {
                        table.AddRow(
                            $"[bold]#{entry.Rank}[/]",
                            Markup.Escape(entry.UserName ?? "Unknown"),
                            entry.Score.ToString()
                        );
                    }
                }

                AnsiConsole.Write(table);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine("[red]Error loading session leaderboard:[/]");
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
            }
        }
    }
}
