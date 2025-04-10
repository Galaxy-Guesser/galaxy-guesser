// src/Data/SampleData.cs
using System.Collections.Generic;
using ConsoleApp1.Models;

namespace ConsoleApp1.Data
{
    public static class UIData
    {
    static readonly string CMD_PREFIX = "/";
    public static readonly Dictionary<string, string> COMMANDS = new Dictionary<string, string>
    {
        { "help", "Show available commands" },
        { "categories", "View all quiz categories" },
        { "sessions", "View all available sessions" },
        { "howtoplay", "Learn how to play Galaxy Quiz" },
        { "leaderboard", "View global leaderboard" },
        { "myprofile", "View your player profile" },
        { "mysessions", "View your session history" },
        { "quit", "Exit the application" },
        { "stats", "View game statistics" }
    };

       
    }
}
