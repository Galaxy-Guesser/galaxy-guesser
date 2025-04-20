namespace ConsoleApp1.Models
{
    public class SessionView
    {
        public int SessionId { get; set; }
        public required string sessionCode { get; set; }
        public required string category { get; set; }
        public required List<string> playerUserNames { get; set; }
        public int playerCount { get; set; }
        public required string duration { get; set; }
        public int questionCount { get; set; }
        public int? highestScore { get; set; }
        public required string endsIn { get; set; }
    }
}