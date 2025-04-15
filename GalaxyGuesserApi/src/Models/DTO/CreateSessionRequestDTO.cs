
namespace GalaxyGuesserApi.Models
{

    public class CreateSessionRequestDTO
    {
        public required string category { get; set; }
        public int questionsCount { get; set; }
        public DateTime startDate { get; set; }
        public decimal sessionDuration { get; set; }
    }
}

