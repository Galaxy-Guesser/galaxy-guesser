using ConsoleApp1.Models;
using ConsoleApp1.Services;
using ConsoleApp1.Utilities;
using Spectre.Console;
using GalaxyGuesserCLI.Services;

namespace ConsoleApp1
{
    class GalaxyQuiz
    {
        public static async Task Start()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "🌌 Galaxy Quiz";
            Console.CursorVisible = false;

           try
            {
                var httpClient = new HttpClient();
                var nasaService = new NasaService(httpClient);
                
                UIService.PrintGalaxyHeader();
                var authService = new AuthenticationService();
                var jwt = await authService.AuthenticateWithGoogle();
                
                var player = await authService.AuthOrRegisterWithBackend();

                var fact = await nasaService.GetSpaceFactAsync();
                
                UIService.DisplaySpaceFact(fact);

                await MainMenuLoop(player);
            }
            catch (Exception exception)
            {
                ErrorHandler.HandleError(exception);
            }
        }

        static async Task MainMenuLoop(Player player)
        {

            bool exitRequested = false;

           while (!exitRequested)
{
    AnsiConsole.Clear();
    
    UIService.PrintGalaxyHeader();
    AnsiConsole.MarkupLine($"\n👋 Welcome, [bold]{player.userName}[/]!\n");
    
    var menuActions = new Dictionary<string, Func<Task>>
    {
        ["Create new quiz session"] = async () => {
            var (category, questionCount, startTime, questionDuration) = await SessionUIService.PromptSessionDetails();
            AnsiConsole.MarkupLine($"Category: [cyan]{category}[/], Question Count: [cyan]{questionCount}[/], Start Time: [cyan]{startTime}[/]");
            
            string sessionCode = await SessionService.CreateSessionAsync(category, questionCount, startTime, questionDuration);
            
            if (!string.IsNullOrEmpty(sessionCode))
                AnsiConsole.MarkupLine($"✅ Session created! Code: [bold yellow]{sessionCode}[/]");
            else
                AnsiConsole.MarkupLine("[red]❌ Failed to create session.[/]");
        },
        
        ["Join existing session"] = async () => {
            var activeSessions = await SessionViewService.GetActiveSessions();
            
            if (activeSessions.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No active sessions available.[/]");
                return;
            }
            
            var selectedSession = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a session to join")
                    .PageSize(10)
                    .AddChoices(activeSessions.Select(s => $"{s.sessionCode} - {s.category}")));
            
            string sessionCode = selectedSession.Split(" - ")[0];
            await SessionService.JoinSessionAsync(sessionCode);
        },
        
        ["View active sessions"] = async () => {
            var sessions = await SessionViewService.GetActiveSessions();
            await UIService.DisplayActiveSessionsAsync(sessions);
        },
        
        // ["View leaderboard"] = () => {
        //     CommandService.ProcessCommand("leaderboard", player);
        //     return Task.CompletedTask;
        // },
        
        // ["My profile"] = () => {
        //     CommandService.ProcessCommand("myprofile", player);
        //     return Task.CompletedTask;
        // },
        
        ["How to play"] = () => {
            UIService.ShowHowToPlay();
            return Task.CompletedTask;
        },
        
        ["Exit"] = () => {
            if (AnsiConsole.Confirm("Are you sure you want to exit?"))
            {
                exitRequested = true;
                AnsiConsole.MarkupLine("\n[green]👋 Thanks for playing Galaxy Quiz! See you among the stars![/]");
                Thread.Sleep(2000);
            }
            return Task.CompletedTask;
        }
    };
    
    // Create menu from the dictionary keys
    var selection = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("MAIN MENU")
            .PageSize(10)
            .AddChoices(menuActions.Keys));
    
    // Execute the selected action
    await menuActions[selection]();
    
    // Continue prompt after each action (unless exiting)
    if (!exitRequested)
        UIService.Continue();
}
        // static void PlayQuizSession(Player player, Session session)
        // {
        //     List<Question> sessionQuestions = SessionService.GetSessionQuestions(session.Id);
        //     int score = 0;
        //     int totalTimeRemaining = 0;

        //     for (int i = 0; i < sessionQuestions.Count; i++)
        //     {
        //         bool answered = false;
        //         int selectedOption = -1;
        //         int timeRemaining = session.QuestionDuration;

        //         // First render the full question and options (once only)
        //         DisplayFullQuestion(sessionQuestions[i], i + 1, sessionQuestions.Count, timeRemaining);

        //         // Display timer
        //         int timerRow = Console.CursorTop - sessionQuestions[i].Options.Length - 4;

        //         // Start timer display thread
        //         CancellationTokenSource cts = new CancellationTokenSource();
        //         Task timerTask = Task.Run(() =>
        //         {
        //             try
        //             {
        //                 while (timeRemaining > 0 && !cts.Token.IsCancellationRequested)
        //                 {
        //                     UIService.UpdateTimerOnly(timerRow, timeRemaining, session.QuestionDuration);
        //                     Thread.Sleep(1000);
        //                     timeRemaining--;
        //                 }
        //             }
        //             catch (OperationCanceledException)
        //             {
        //                 // Task was canceled, do nothing
        //             }
        //         }, cts.Token);

        //         Task<(bool answered, int selectedOption)> answerTask = SessionService.WaitForAnswerWithTimeout(
        //             sessionQuestions[i], session.QuestionDuration);
        //         answerTask.Wait();

        //         cts.Cancel();

        //         answered = answerTask.Result.answered;
        //         selectedOption = answerTask.Result.selectedOption;

        //         int inputRow = Console.CursorTop;
        //         Console.SetCursorPosition(0, inputRow);
        //         Console.Write(new string(' ', Console.WindowWidth));
        //         Console.SetCursorPosition(0, inputRow);

        //         if (answered)
        //         {
        //             Console.Write($"👉 Your answer: {(char)('A' + selectedOption)}");

        //             if (selectedOption == sessionQuestions[i].CorrectAnswerIndex)
        //             {
        //                 score++;
        //                 totalTimeRemaining += timeRemaining; // Award bonus points for quick answers
        //                 UIService.ShowFeedback($"✅ Correct! +{timeRemaining} time bonus!", ConsoleColor.Green);
        //             }
        //             else
        //             {
        //                 UIService.ShowFeedback($"❌ Wrong! Correct was {(char)('A' + sessionQuestions[i].CorrectAnswerIndex)}",
        //                            ConsoleColor.Red);
        //             }
        //         }
        //         else
        //         {
        //             UIService.ShowFeedback($"⏰ Time's up! Correct was {(char)('A' + sessionQuestions[i].CorrectAnswerIndex)}",
        //                        ConsoleColor.Yellow);
        //         }

        //         Thread.Sleep(1500);
        //     }

        //     // Save score
        //     SessionService.SaveScore(player.playerId, session.Id, score, totalTimeRemaining);
        // }

        static void DisplayFullQuestion(Question q, int current, int total, int secondsRemaining)
        {
            Console.Clear();
            UIService.PrintGalaxyHeader();

            // Placeholder for timer bar - will be updated separately
            Console.WriteLine($"⏱ Time: {secondsRemaining}s [" + new string(' ', Console.WindowWidth - 20) + "]");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nQuestion {current}/{total}:");
            Console.ResetColor();

            // Make the question text much more visible with highlighting
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n{q.Text}\n");
            Console.ResetColor();
            Console.WriteLine(); // Extra spacing
            // Display answer options with clear formatting
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
    }
}
}