// src/Models/SessionScore.cs
namespace ConsoleApp1.Models
{
    public class SessionScore
    {
        public int PlayerId { get; set; }
        public int SessionId { get; set; }
        public int Score { get; set; }
        public int TimeRemaining { get; set; } // Store remaining time as bonus points

        public SessionScore(int playerId, int sessionId, int score, int timeRemaining = 0)
        {
            PlayerId = playerId;
            SessionId = sessionId;
            Score = score;
            TimeRemaining = timeRemaining;
        }
    }
}
