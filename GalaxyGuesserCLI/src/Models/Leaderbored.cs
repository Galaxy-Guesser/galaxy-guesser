
namespace GalaxyGuesserCLI.Models
{
        public class Leaderbored
        {
            public class LeaderboardEntry
        {
            public int PlayerId { get; set; }
            public string UserName { get; set; } = string.Empty;
            public int Score { get; set; }
            public int Rank { get; set; }
        }

        public class GlobalLeaderboardEntry
        {
            public int PlayerId { get; set; }
            public string UserName { get; set; } = string.Empty;
            public int TotalScore { get; set; }
            public int SessionsPlayed { get; set; }
            public int Rank { get; set; }
            public List<string> Sessions { get; set; } = new List<string>();
        }

        public class SessionLeaderboardResponse
        {
            public bool Success { get; set; }
            public string SessionCode { get; set; } = string.Empty;
            public List<LeaderboardEntry> Leaderboard { get; set; } = new List<LeaderboardEntry>();
            public string Message { get; set; } = string.Empty;
        }

        public class GlobalLeaderboardResponse
        {
            public bool Success { get; set; }
            public List<GlobalLeaderboardEntry> Leaderboard { get; set; } = new List<GlobalLeaderboardEntry>();
            public string Message { get; set; } = string.Empty;
        }
    }
}