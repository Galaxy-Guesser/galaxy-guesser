namespace ConsoleApp1.Models
{
    public class SessionView
    {
        public int SessionId { get; set; }
        public string session_code { get; set; }
        public string session_category { get; set; }
        public List<string> PlayerUsernames { get; set; }
        public int PlayerCount { get; set; }
        public string Duration { get; set; }
        public int QuestionCount { get; set; }
        public int? HighestScore { get; set; }
        public string ends_in { get; set; }
    }
}