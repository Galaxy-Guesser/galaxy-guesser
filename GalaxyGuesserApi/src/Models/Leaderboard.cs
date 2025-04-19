namespace GalaxyGuesserApi.Models
{
    public class LeaderboardEntry
    {
        public string SessionCode {get; set;}  = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int Score { get; set; }
        public int Rank { get; set; }
    }

    public class SessionLeaderboardResponse
    {
        public bool Success { get; set; }
        public string SessionCode { get; set; } = string.Empty;
        public List<LeaderboardEntry> Leaderboard { get; set; } = new();
        public string Message { get; set; } = string.Empty;
    }

    public class GlobalLeaderboardEntry
    {
        public int PlayerId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int TotalScore { get; set; }
        public int SessionsPlayed { get; set; }
        public int Rank { get; set; }
        public List<string> Sessions { get; set; } = new();
    }

    public class GlobalLeaderboardResponse
    {
        public bool Success { get; set; }
        public List<GlobalLeaderboardEntry> Leaderboard { get; set; } = new();
        public string Message { get; set; } = string.Empty;
    }
}
