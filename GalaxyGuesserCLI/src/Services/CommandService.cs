using System;
using System.Collections.Generic;
using System.Threading;
using GalaxyGuesserCli.Models;
using GalaxyGuesserCli.Data;

namespace GalaxyGuesserCli.Services
{
    public class CommandService
    {
        public static readonly string CMD_PREFIX = "/";

        internal static void ProcessCommand(string command, Player player)
        {
            switch (command)
            {
              
                case "sessions":
                    ViewSessions();
                    break;
                case "howtoplay":
                    UIService.ShowHowToPlay();
                    break;
                case "leaderboard":
                    ViewLeaderboard();
                    break;
                case "myprofile":
                    ViewProfile(player);
                    break;
                case "mysessions":
                    ViewMySessionHistory(player);
                    break;
                case "quit":
                    Console.WriteLine("\n👋 Thanks for playing Galaxy Quiz! See you among the stars!");
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                    break;
                case "stats":
                    ViewGameStats();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Unknown command: {command}");
                    Console.WriteLine("Type '/help' to see available commands.");
                    Console.ResetColor();
                    break;
            }
        }

       

        private static void ViewSessions()
        {
            Console.WriteLine("\nThis feature is not yet implemented.");
        }

        private static void ViewLeaderboard()
        {
            Console.WriteLine("\nThis feature is not yet implemented.");
        }

        private static void ViewProfile(Player player)
        {
            Console.WriteLine($"\nProfile for {player.userName}:");
            Console.WriteLine($"Player ID: {player.playerId}");
            Console.WriteLine($"GUID: {player.guid}");
        }

        private static void ViewMySessionHistory(Player player)
        {
            Console.WriteLine("\nThis feature is not yet implemented.");
        }

        private static void ViewGameStats()
        {
            Console.WriteLine("\nThis feature is not yet implemented.");
        }

        public static bool IsCommand(string input)
        {
            return input.StartsWith(CMD_PREFIX);
        }

        public static string ExtractCommandName(string input)
        {
            return input.Substring(1).ToLower();
        }
    }
}