
namespace GalaxyGuesserApi.Models
{

    public class CreateSessionRequestDTO
    {
        public required string category { get; set; }
        public int questionsCount { get; set; }
        public required string userGuid { get; set; }
        public DateTime startDate { get; set; }
        public int questionDuration { get; set; }
    }
}

