using System;
using System.Collections.Generic;

namespace GalaxyGuesserApi.Models
{
    public class SessionView
    {
        public int sessionId{ get; set; }
        public string sessionCode { get; set; } = null!;
        public string category { get; set; } = null!;
        public List<string> playerUserNames { get; set; } = new();
        public int playerCount{ get; set; }
        public int questionCount{ get; set; }
        public int? highestScore { get; set; }
        public string? endsIn{ get; set; }
    }
}
