using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using ConsoleApp1.Models;
using ConsoleApp1.Services;
using ConsoleApp1.Data;


namespace ConsoleApp1{
class GalaxyQuiz
{
    // In-memory data storage
    static List<Player> players = new List<Player>();
    static List<Session> sessions = new List<Session>();
    static List<Category> categories = SampleData.Categories;
    static List<SessionPlayer> sessionPlayers = new List<SessionPlayer>();
    static List<SessionQuestion> sessionQuestions = new List<SessionQuestion>();
    static List<Question> questions = SampleData.Questions;
    static List<SessionScore> sessionScores = new List<SessionScore>();
    static Dictionary<string, string> userCredentials = SampleData.UserCredentials; // Simple login storage

    static readonly string CMD_PREFIX = "/";
    static readonly Dictionary<string, string> COMMANDS =UIData.COMMANDS;

    public static void Start()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "🌌 Galaxy Quiz";
        Console.CursorVisible = false;

        try
        {
            UIService.PrintGalaxyHeader();
            Player player = AuthenticatePlayer();
            MainMenuLoop(player);
            // Session session = CreateOrJoinSession(player);
            // PlayQuizSession(player, session);
            
            // // Final results
            // ShowFinalResults(player, session);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"🚨 Error: {ex.Message}");
            Console.ResetColor();
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

      static void MainMenuLoop(Player player)
    {
        bool exitRequested = false;
        
        while (!exitRequested)
        {
            UIService.PrintGalaxyHeader();
            Console.WriteLine($"\n👋 Welcome, {player.Name}!");
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine("1. Create new quiz session");
            Console.WriteLine("2. Join existing session");
            Console.WriteLine("3. View categories");
            Console.WriteLine("4. View leaderboard");
            Console.WriteLine("5. My profile");
            Console.WriteLine("6. How to play");
            Console.WriteLine("7. Exit");
            Console.WriteLine("\nType a command (e.g., '/help') or select an option (1-7):");
            
            Console.Write("\n👉 ");
            Console.CursorVisible = true;
            string input = Console.ReadLine().Trim();
            Console.CursorVisible = false;
            
            // Check if input is a command
            if (input.StartsWith(CMD_PREFIX))
            {
                string cmd = input.Substring(1).ToLower();
                ProcessCommand(cmd, player);
                UIService.Continue();
            }
            else if (int.TryParse(input, out int option))
            {
                switch (option)
                {
                    case 1:
                        Session newSession = CreateOrJoinSession(player);
                        if (newSession != null)
                        {
                            PlayQuizSession(player, newSession);
                            ShowFinalResults(player, newSession);
                        }
                        UIService.Continue();
                        break;
                    case 2:
                        Session joinSession = JoinSession(player);
                        if (joinSession != null)
                        {
                            PlayQuizSession(player, joinSession);
                            ShowFinalResults(player, joinSession);
                        }
                        UIService.Continue();
                        break;
                    // case 3:
                    //     ViewCategories();
                    //     Continue();
                    //     break;
                    // case 4:
                    //     ViewLeaderboard();
                    //     Continue();
                    //     break;
                    // case 5:
                    //     ViewProfile(player);
                    //     Continue();
                    //     break;
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


    static void ProcessCommand(string command, Player player)
    {
        switch (command)
        {
            case "help":
                UIService.ShowHelp(COMMANDS);
                break;
            // case "categories":
            //     ViewCategories();
            //     break;
            // case "sessions":
            //     ViewSessions();
            //     break;
            case "howtoplay":
                UIService.ShowHowToPlay();
                break;
            // case "leaderboard":
            //     ViewLeaderboard();
            //     break;
            // case "myprofile":
            //     ViewProfile(player);
            //     break;
            // case "mysessions":
            //     ViewMySessionHistory(player);
            //     break;
            case "quit":
                Console.WriteLine("\n👋 Thanks for playing Galaxy Quiz! See you among the stars!");
                Thread.Sleep(2000);
                Environment.Exit(0);
                break;
            // case "stats":
            //     ViewGameStats();
            //     break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Unknown command: {command}");
                Console.WriteLine("Type '/help' to see available commands.");
                Console.ResetColor();
                break;
        }
    }


    static Session CreateSession(Player player)
{
    Console.WriteLine("\nAvailable categories:");
    for (int i = 0; i < categories.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {categories[i].Name}");
    }

    Console.Write("\n👉 Select category (1-" + categories.Count + "): ");
    Console.CursorVisible = true;
    int catChoice;
    while (!int.TryParse(Console.ReadLine(), out catChoice) || catChoice < 1 || catChoice > categories.Count)
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

    string sessionCode = SessionService.GenerateSessionCode();
    int sessionId = sessions.Count > 0 ? sessions.Max(s => s.Id) + 1 : 1;

    Session session = new Session(sessionId, sessionCode, categories[catChoice - 1].Id, questionDuration, questionCount);
    sessions.Add(session);

    AddQuestionsToSession(session.Id, categories[catChoice - 1].Id, questionCount);
    AddPlayerToSession(player.Id, session.Id);

    Console.WriteLine($"\n🎮 Session created! Code: {sessionCode}");
    Thread.Sleep(2000);

    return session;
}

static Session JoinSession(Player player)
{
    Console.Write("\n👉 Enter session code: ");
    Console.CursorVisible = true;
    string sessionCode = Console.ReadLine().ToUpper();
    Console.CursorVisible = false;

    Session session = sessions.FirstOrDefault(s => s.Code == sessionCode);

    if (session != null)
    {
        AddPlayerToSession(player.Id, session.Id);

        Console.WriteLine("\n🚀 Joining session...");
        Thread.Sleep(1500);

        return session;
    }
    else
    {
        throw new Exception("Session not found!");
    }
}

    static Player AuthenticatePlayer()
    {
        while (true)
        {
            Console.WriteLine("\n1. Login\n2. Register new account");
            Console.Write("\n👉 Enter your choice (1 or 2): ");
            Console.CursorVisible = true;
            
            ConsoleKey choice = Console.ReadKey(true).Key;
            Console.CursorVisible = false;
            
            if (choice == ConsoleKey.D1)
            {
                Console.WriteLine("1");
                // Login
                Console.Write("\nUsername: ");
                Console.CursorVisible = true;
                string username = Console.ReadLine();
                Console.CursorVisible = false;
                
                Console.Write("Password: ");
                Console.CursorVisible = true;
                string password = ReadPassword();
                Console.CursorVisible = false;

                if (userCredentials.TryGetValue(username, out string storedPassword) && password == storedPassword)
                {
                    // Find existing player
                    Player existingPlayer = players.FirstOrDefault(p => p.Username == username);
                    if (existingPlayer != null)
                    {
                        Console.WriteLine($"\n👋 Welcome back, {existingPlayer.Name}!");
                        Thread.Sleep(1500);
                        return existingPlayer;
                    }
                    else
                    {
                        // Create new player profile
                        Console.Write("\nEnter your display name: ");
                        Console.CursorVisible = true;
                        string name = Console.ReadLine();
                        Console.CursorVisible = false;

                        int playerId = players.Count > 0 ? players.Max(p => p.Id) + 1 : 1;
                        Guid playerGuid = Guid.NewGuid();

                        Player player = new Player(playerId, playerGuid, username, name);
                        players.Add(player);

                        Console.WriteLine($"\n👋 Welcome, {player.Name}!");
                        Thread.Sleep(1500);
                        return player;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n❌ Invalid username or password. Try again.");
                    Console.ResetColor();
                }
            }
            else if (choice == ConsoleKey.D2)
            {
                Console.WriteLine("2");
                // Register
                Console.Write("\nCreate username: ");
                Console.CursorVisible = true;
                string username = Console.ReadLine();
                Console.CursorVisible = false;

                if (userCredentials.ContainsKey(username))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Username already exists. Try another one.");
                    Console.ResetColor();
                    continue;
                }

                Console.Write("Create password: ");
                Console.CursorVisible = true;
                string password = ReadPassword();
                Console.CursorVisible = false;
                
                Console.Write("\nEnter your display name: ");
                Console.CursorVisible = true;
                string name = Console.ReadLine();
                Console.CursorVisible = false;

                // Save credentials
                userCredentials.Add(username, password);

                // Create player
                int playerId = players.Count > 0 ? players.Max(p => p.Id) + 1 : 1;
                Guid playerGuid = Guid.NewGuid();

                Player player = new Player(playerId, playerGuid, username, name);
                players.Add(player);

                Console.WriteLine($"\n✅ Registration successful! Welcome, {player.Name}!");
                Thread.Sleep(1500);
                return player;
            }
        }
    }

    static string ReadPassword()
    {
        StringBuilder password = new StringBuilder();
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
            {
                password.Append(key.KeyChar);
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password.Remove(password.Length - 1, 1);
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return password.ToString();
    }

    static Session CreateOrJoinSession(Player player)
    {
        Console.WriteLine("\n1. Create new session\n2. Join existing session");
        Console.Write("\n👉 Enter your choice (1 or 2): ");
        Console.CursorVisible = true;
        
        ConsoleKey choice;
        do
        {
            choice = Console.ReadKey(true).Key;
        } while (choice != ConsoleKey.D1 && choice != ConsoleKey.D2);
        
        Console.WriteLine(choice == ConsoleKey.D1 ? "1" : "2");
        Console.CursorVisible = false;

        if (choice == ConsoleKey.D1)
        {
            // Create new session
            Console.WriteLine("\nAvailable categories:");
            
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i+1}. {categories[i].Name}");
            }

            Console.Write("\n👉 Select category (1-" + categories.Count + "): ");
            Console.CursorVisible = true;
            int catChoice;
            while (!int.TryParse(Console.ReadLine(), out catChoice) || catChoice < 1 || catChoice > categories.Count)
            {
                Console.Write("Invalid choice. Please select again: ");
            }
            Console.CursorVisible = false;

            // Get question count
            Console.Write("\n👉 How many questions do you want (1-10)? ");
            Console.CursorVisible = true;
            int questionCount;
            while (!int.TryParse(Console.ReadLine(), out questionCount) || questionCount < 1 || questionCount > 10)
            {
                Console.Write("Please enter a number between 1 and 10: ");
            }
            Console.CursorVisible = false;

            // Get time per question
            Console.Write("\n👉 Time per question in seconds (10-60): ");
            Console.CursorVisible = true;
            int questionDuration;
            while (!int.TryParse(Console.ReadLine(), out questionDuration) || questionDuration < 10 || questionDuration > 60)
            {
                Console.Write("Please enter a number between 10 and 60: ");
            }
            Console.CursorVisible = false;

            string sessionCode = SessionService.GenerateSessionCode();
            int sessionId = sessions.Count > 0 ? sessions.Max(s => s.Id) + 1 : 1;
            
            Session session = new Session(sessionId, sessionCode, categories[catChoice-1].Id, questionDuration, questionCount);
            sessions.Add(session);
            
            // Add questions to session
            AddQuestionsToSession(session.Id, categories[catChoice-1].Id, questionCount);
            
            // Add player to session
            AddPlayerToSession(player.Id, session.Id);
            
            Console.WriteLine($"\n🎮 Session created! Code: {sessionCode}");
            Thread.Sleep(2000);
            
            return session;
        }
        else
        {
            // Join existing session
            Console.Write("\n👉 Enter session code: ");
            Console.CursorVisible = true;
            string sessionCode = Console.ReadLine().ToUpper();
            Console.CursorVisible = false;
            
            Session session = sessions.FirstOrDefault(s => s.Code == sessionCode);
            
            if (session != null)
            {
                // Add player to session
                AddPlayerToSession(player.Id, session.Id);
                
                Console.WriteLine("\n🚀 Joining session...");
                Thread.Sleep(1500);
                
                return session;
            }
            else
            {
                throw new Exception("Session not found!");
            }
        }
    }

    static async Task<(bool answered, int selectedOption)> WaitForAnswerWithTimeout(Question question, int timeoutSeconds)
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

    static void PlayQuizSession(Player player, Session session)
    {
        List<Question> sessionQuestions = GetSessionQuestions(session.Id);
        int score = 0;
        int totalTimeRemaining = 0;

        for (int i = 0; i < sessionQuestions.Count; i++)
        {
            bool answered = false;
            int selectedOption = -1;
            int timeRemaining = session.QuestionDuration;

            // First render the full question and options (once only)
            DisplayFullQuestion(sessionQuestions[i], i+1, sessionQuestions.Count, timeRemaining);

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
                        UpdateTimerOnly(timerRow, timeRemaining, session.QuestionDuration);
                        Thread.Sleep(1000);
                        timeRemaining--;
                    }
                }
                catch (OperationCanceledException)
                {
                    // Task was canceled, do nothing
                }
            }, cts.Token);

            // Wait for answer or timeout
            Task<(bool answered, int selectedOption)> answerTask = WaitForAnswerWithTimeout(
                sessionQuestions[i], session.QuestionDuration);
            answerTask.Wait();
            
            // Stop timer thread
            cts.Cancel();
            
            // Get results
            answered = answerTask.Result.answered;
            selectedOption = answerTask.Result.selectedOption;
            
            // Update the input line with the selected answer
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
        SaveScore(player.Id, session.Id, score, totalTimeRemaining);
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

    static void UpdateTimerOnly(int row, int secondsRemaining, int totalSeconds)
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

    static void ShowFinalResults(Player player, Session session)
    {
        Console.Clear();
        UIService.PrintGalaxyHeader();

        // Get player's score
        SessionScore playerScore = sessionScores.FirstOrDefault(s => s.PlayerId == player.Id && s.SessionId == session.Id);
        int score = playerScore != null ? playerScore.Score : 0;
        int timeBonus = playerScore != null ? playerScore.TimeRemaining : 0;

        // Get total questions
        int totalQuestions = sessionQuestions.Count(sq => sq.SessionId == session.Id);

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"\n🌟 Final Score: {score}/{totalQuestions} correct answers");
        Console.WriteLine($"⏱ Time Bonus: {timeBonus} points");
        Console.WriteLine($"🏆 Total Score: {score + timeBonus} points\n");

        // Get leaderboard
        Console.WriteLine("🏆 Leaderboard:");
        var leaderboard = sessionScores
            .Where(s => s.SessionId == session.Id)
            .OrderByDescending(s => s.Score + s.TimeRemaining)
            .Select(s => new { 
                Name = players.First(p => p.Id == s.PlayerId).Name, 
                Score = s.Score,
                TimeBonus = s.TimeRemaining,
                Total = s.Score + s.TimeRemaining
            })
            .ToList();
            
        for (int i = 0; i < leaderboard.Count; i++)
        {
            var entry = leaderboard[i];
            Console.WriteLine($"{i+1}. {entry.Name}: {entry.Score} correct + {entry.TimeBonus} time bonus = {entry.Total} points");
        }

        Console.ResetColor();
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    #region Data Helpers
    static void AddPlayerToSession(int playerId, int sessionId)
    {
        // Check if player already in session
        if (!sessionPlayers.Any(sp => sp.PlayerId == playerId && sp.SessionId == sessionId))
        {
            sessionPlayers.Add(new SessionPlayer(sessionId, playerId));
        }
    }

    static void AddQuestionsToSession(int sessionId, int categoryId, int questionCount)
    {
        // Get questions for this category and randomize them
        var categoryQuestions = questions
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

    static List<Question> GetSessionQuestions(int sessionId)
    {
        return sessionQuestions
            .Where(sq => sq.SessionId == sessionId)
            .Join(
                questions, 
                sq => sq.QuestionId, 
                q => q.Id, 
                (sq, q) => q
            )
            .ToList();
    }

    static void SaveScore(int playerId, int sessionId, int score, int timeRemaining = 0)
    {
        sessionScores.Add(new SessionScore(playerId, sessionId, score, timeRemaining));
    }
    #endregion
}
}