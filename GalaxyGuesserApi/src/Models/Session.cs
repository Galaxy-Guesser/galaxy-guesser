using System;

namespace GalaxyGuesserApi.Models
{
    public class Session
    {
        public int sessionId { get; private set; }
        public required string sessionCode { get; set; }
        public required string category { get; set; }
        public DateTime startDate { get; private set; }
        public int endDate { get; private set; }
    }
}