using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using ConsoleApp1.Models;
using ConsoleApp1.Services;
using ConsoleApp1.Data;
using ConsoleApp1.Helpers;

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
                UIService.PrintGalaxyHeader();
                Console.WriteLine(await CategoryService.GetCategoriesAsync());
                Console.WriteLine($"\n👋 Welcome, {player.userName}!");
                Console.WriteLine("\nMAIN MENU");
                Console.WriteLine("1. Create new quiz session");
                Console.WriteLine("2. Join existing session");
                Console.WriteLine("3. View active sessions");
                Console.WriteLine("4. View categories");
                Console.WriteLine("5. View leaderboard");
                Console.WriteLine("6. My profile");
                Console.WriteLine("7. How to play");
                Console.WriteLine("8. Exit");
                Console.WriteLine("\nType a command (e.g., '/help') or select an option (1-7):");

                Console.Write("\n👉 ");
                Console.CursorVisible = true;
                string input = Console.ReadLine().Trim();
                Console.CursorVisible = false;

                // Check if input is a command
                if (CommandService.IsCommand(input))
                {
                    string cmd = CommandService.ExtractCommandName(input);
                    CommandService.ProcessCommand(cmd, player);
                    UIService.Continue();
                }
                else if (int.TryParse(input, out int option))
                {
                    switch (option)
                    {
                        case 1:
                            var (category, questionCount, startTime,sessionDuration) = SessionUIService.PromptSessionDetails();
                            Console.WriteLine($"Category: {category}, Question Count: {questionCount}, Start Time: {startTime}");

                            string sessionCode = await SessionService.CreateSessionAsync(category, questionCount,startTime, sessionDuration);

                            if (!string.IsNullOrEmpty(sessionCode))
                            {
                                UIService.ShowFeedback($"✅ Session created! Code: [bold yellow]{sessionCode}[/]", ConsoleColor.Green);
                            }
                            else
                            {
                                UIService.ShowFeedback("❌ Failed to create session.", ConsoleColor.Red);
                            }

                            UIService.Continue();
                            break;
                        case 2:
                            var activeSessions = await SessionViewService.GetActiveSessions();

                            if (activeSessions.Count == 0)
                            {
                                AnsiConsole.MarkupLine("[red]No active sessions available.[/]");
                                break;
                            }

                            var selected = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .Title("Select a session to join")
                                    .PageSize(10)
                                    .AddChoices(activeSessions.Select(s =>
                                        $"{s.sessionCode} - {s.category}")));

                            sessionCode = selected.Split(" - ")[0];
                            await SessionService.JoinSessionAsync(sessionCode);

                            UIService.Continue();
                            break;


                        case 3:
                            var sessions = await SessionViewService.GetActiveSessions();
                            UIService.DisplayActiveSessionsAsync(sessions);
                            UIService.Continue();
                            break;

                        case 4:
                            CommandService.ProcessCommand("leaderboard", player);
                            UIService.Continue();
                            break;
                        case 5:
                            CommandService.ProcessCommand("myprofile", player);
                            UIService.Continue();
                            break;
                        case 6:
                            UIService.ShowHowToPlay();
                            UIService.Continue();
                            break;
                        case 7:
                            exitRequested = true;
                            Console.WriteLine("\n👋 Thanks for playing Galaxy Quiz! See you among the stars!");
                            Thread.Sleep(2000);
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            UIService.Continue();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number or command.");
                    UIService.Continue();
                }
            }
        }

        static Session CreateSession(Player player)
        {
            Console.WriteLine("\nAvailable categories:");
            for (int i = 0; i < SampleData.Categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {SampleData.Categories[i].category}");
            }

            Console.Write("\n👉 Select category (1-" + SampleData.Categories.Count + "): ");
            Console.CursorVisible = true;
            int catChoice;
            while (!int.TryParse(Console.ReadLine(), out catChoice) || catChoice < 1 || catChoice > SampleData.Categories.Count)
            {
                Console.Write("Invalid choice. Please select again: ");
            }

            Console.Write("\n👉 How many questions do you want (1-10)? ");
            int questionCount;
            while (!int.TryParse(Console.ReadLine(), out questionCount) || questionCount < 1 || questionCount > 10)
            {
                Console.Write("Please enter a number between 1 and 10: ");
            }

            Console.Write("\n👉 Time per question in seconds (10-60): ");
            int questionDuration;
            while (!int.TryParse(Console.ReadLine(), out questionDuration) || questionDuration < 10 || questionDuration > 60)
            {
                Console.Write("Please enter a number between 10 and 60: ");
            }
            Console.CursorVisible = false;

            Session session = SessionService.CreateSession(
                player,
                SampleData.Categories[catChoice - 1].categoryId,
                questionCount,
                questionDuration
            );

            Console.WriteLine($"\n🎮 Session created! Code: {session.Code}");
            Thread.Sleep(2000);

            return session;
        }

        static Session JoinSession(Player player)
        {
            Console.Write("\n👉 Enter session code: ");
            Console.CursorVisible = true;
            string sessionCode = Console.ReadLine().ToUpper();
            Console.CursorVisible = false;

            Session session = SessionService.JoinSession(player, sessionCode);

            if (session != null)
            {
                Console.WriteLine("\n🚀 Joining session...");
                Thread.Sleep(1500);
                return session;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Session not found. Please check the code and try again.");
                Console.ResetColor();
                return null;
            }
        }

        static void PlayQuizSession(Player player, Session session)
        {
            List<Question> sessionQuestions = SessionService.GetSessionQuestions(session.Id);
            int score = 0;
            int totalTimeRemaining = 0;

            for (int i = 0; i < sessionQuestions.Count; i++)
            {
                bool answered = false;
                int selectedOption = -1;
                int timeRemaining = session.QuestionDuration;

                // First render the full question and options (once only)
                DisplayFullQuestion(sessionQuestions[i], i + 1, sessionQuestions.Count, timeRemaining);

                // Display timer
                int timerRow = Console.CursorTop - sessionQuestions[i].Options.Length - 4;

                // Start timer display thread
                CancellationTokenSource cts = new CancellationTokenSource();
                Task timerTask = Task.Run(() =>
                {
                    try
                    {
                        while (timeRemaining > 0 && !cts.Token.IsCancellationRequested)
                        {
                            UIService.UpdateTimerOnly(timerRow, timeRemaining, session.QuestionDuration);
                            Thread.Sleep(1000);
                            timeRemaining--;
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // Task was canceled, do nothing
                    }
                }, cts.Token);

                Task<(bool answered, int selectedOption)> answerTask = SessionService.WaitForAnswerWithTimeout(
                    sessionQuestions[i], session.QuestionDuration);
                answerTask.Wait();

                cts.Cancel();

                answered = answerTask.Result.answered;
                selectedOption = answerTask.Result.selectedOption;

                int inputRow = Console.CursorTop;
                Console.SetCursorPosition(0, inputRow);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, inputRow);

                if (answered)
                {
                    Console.Write($"👉 Your answer: {(char)('A' + selectedOption)}");

                    if (selectedOption == sessionQuestions[i].CorrectAnswerIndex)
                    {
                        score++;
                        totalTimeRemaining += timeRemaining; // Award bonus points for quick answers
                        UIService.ShowFeedback($"✅ Correct! +{timeRemaining} time bonus!", ConsoleColor.Green);
                    }
                    else
                    {
                        UIService.ShowFeedback($"❌ Wrong! Correct was {(char)('A' + sessionQuestions[i].CorrectAnswerIndex)}",
                                   ConsoleColor.Red);
                    }
                }
                else
                {
                    UIService.ShowFeedback($"⏰ Time's up! Correct was {(char)('A' + sessionQuestions[i].CorrectAnswerIndex)}",
                               ConsoleColor.Yellow);
                }

                Thread.Sleep(1500);
            }

            // Save score
            SessionService.SaveScore(player.playerId, session.Id, score, totalTimeRemaining);
        }
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