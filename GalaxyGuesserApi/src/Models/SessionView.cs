using System;
using System.Collections.Generic;

namespace GalaxyGuesserApi.Models
{
    public class SessionView
    {
        public int session_id { get; set; }
        public string session_code { get; set; } = null!;
        public string session_category { get; set; } = null!;
        public List<string> player_usernames { get; set; } = new();
        public int player_count { get; set; }
        public string duration { get; set; } = null!;
        public int question_count { get; set; }
        public int? highest_score { get; set; }
        public string? ends_in { get; set; }
    }
}
