
namespace GalaxyGuesserApi.Models
{
    public class Session
    {
        public int sessionId { get; set; }
        public required string sessionCode { get; set; }
        public required int categoryId { get; set; }
        public required DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}