// using System;
// using System.Text;
// using ConsoleApp1.Models.Player;

// namespace ConsoleApp1.Services
// {
//     public class AuthenticationService
//     {
//         private readonly DataService _dataService;
        
//         public AuthenticationService(DataService dataService)
//         {
//             _dataService = dataService;
//         }
        
//         public Player AuthenticatePlayer()
//         {
//             while (true)
//             {
//                 Console.WriteLine("\n1. Login\n2. Register new account");
//                 Console.Write("\n👉 Enter your choice (1 or 2): ");
//                 Console.CursorVisible = true;
                
//                 ConsoleKey choice = Console.ReadKey(true).Key;
//                 Console.CursorVisible = false;
                
//                 if (choice == ConsoleKey.D1)
//                 {
//                     Console.WriteLine("1");
//                     // Login
//                     Console.Write("\nUsername: ");
//                     Console.CursorVisible = true;
//                     string username = Console.ReadLine();
//                     Console.CursorVisible = false;
                    
//                     Console.Write("Password: ");
//                     Console.CursorVisible = true;
//                     string password = ReadPassword();
//                     Console.CursorVisible = false;

//                     if (_dataService.ValidateCredentials(username, password))
//                     {
//                         // Find existing player
//                         Player existingPlayer = _dataService.GetPlayerByUsername(username);
//                         if (existingPlayer != null)
//                         {
//                             Console.WriteLine($"\n👋 Welcome back, {existingPlayer.Name}!");
//                             Thread.Sleep(1500);
//                             return existingPlayer;
//                         }
//                         else
//                         {
//                             // Create new player profile
//                             Console.Write("\nEnter your display name: ");
//                             Console.CursorVisible = true;
//                             string name = Console.ReadLine();
//                             Console.CursorVisible = false;

//                             Player player = _dataService.CreatePlayer(username, name);

//                             Console.WriteLine($"\n👋 Welcome, {player.Name}!");
//                             Thread.Sleep(1500);
//                             return player;
//                         }
//                     }
//                     else
//                     {
//                         Console.ForegroundColor = ConsoleColor.Red;
//                         Console.WriteLine("\n❌ Invalid username or password. Try again.");
//                         Console.ResetColor();
//                     }
//                 }
//                 else if (choice == ConsoleKey.D2)
//                 {
//                     Console.WriteLine("2");
//                     // Register
//                     Console.Write("\nCreate username: ");
//                     Console.CursorVisible = true;
//                     string username = Console.ReadLine();
//                     Console.CursorVisible = false;

//                     if (_dataService.UserExists(username))
//                     {
//                         Console.ForegroundColor = ConsoleColor.Red;
//                         Console.WriteLine("❌ Username already exists. Try another one.");
//                         Console.ResetColor();
//                         continue;
//                     }

//                     Console.Write("Create password: ");
//                     Console.CursorVisible = true;
//                     string password = ReadPassword();
//                     Console.CursorVisible = false;
                    
//                     Console.Write("\nEnter your display name: ");
//                     Console.CursorVisible = true;
//                     string name = Console.ReadLine();
//                     Console.CursorVisible = false;

//                     // Save credentials and create player
//                     _dataService.AddCredentials(username, password);
//                     Player player = _dataService.CreatePlayer(username, name);

//                     Console.WriteLine($"\n✅ Registration successful! Welcome, {player.Name}!");
//                     Thread.Sleep(1500);
//                     return player;
//                 }
//             }
//         }
        
//         private string ReadPassword()
//         {
//             StringBuilder password = new StringBuilder();
//             ConsoleKeyInfo key;

//             do
//             {
//                 key = Console.ReadKey(true);

//                 if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
//                 {
//                     password.Append(key.KeyChar);
//                     Console.Write("*");
//                 }
//                 else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
//                 {
//                     password.Remove(password.Length - 1, 1);
//                     Console.Write("\b \b");
//                 }
//             } while (key.Key != ConsoleKey.Enter);

//             Console.WriteLine();
//             return password.ToString();
//         }
//     }
// }