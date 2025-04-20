
namespace GalaxyGuesserApi.Models.DTO
{

    public class CreateSessionRequestDTO
    {
        public required int categoryId { get; set; }
        public int questionsCount { get; set; }
        public DateTime startDate { get; set; }
        public decimal sessionDuration { get; set; }
    }
}

