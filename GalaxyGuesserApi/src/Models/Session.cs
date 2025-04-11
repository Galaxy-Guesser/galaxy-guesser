using System;

namespace GalaxyGuesserApi.Models
{
    public class Session
    {
        public int session_id { get; private set; }
        public required string session_code { get; set; }
        public required string category { get; set; }
        public DateTime start_date { get; private set; }
        public int end_date { get; private set; }
    }
}