// src/Services/UIService.cs
using System;

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
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(@"
            _____       _____  _______  ______ _____  _____ _    _ 
            / ____|/\   |  __ \|_   _\ \/ / __ \_   _|/ ____| |  | |
            | |  __/  \  | |__) | | |  \  / |  | || | | (___ | |__| |
            | | |_ / /\ \|  _  /  | |  /  \ |  | || |  \___ \|  __  |
            | |__/ / ____ \ | \ \ _| |_/ /\ \ |__| || |_ ____) | |  | |
            \_____/_/    \_\_| \_\_____/_/  \_\____/_____|_____/|_|  |_|
            ");
            Console.ResetColor();
            Console.WriteLine("================================================");
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
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
    }


    }
}
