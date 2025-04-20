namespace GalaxyGuesserCLI.Models
{
    public class SessionModel
    {
        public int sessionId { get; set; }
        public required string sessionCode { get; set; }
        public required int categoryId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}