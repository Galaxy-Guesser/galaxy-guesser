namespace GalaxyGuesserApi.Models
{
    public class SessionDTO
    {
        public string? sessionCode { get; set; }
        public required string category { get; set; }

        public required string user_name { get; set;}

        public  int? questionCount { get; set;} = 3;
        public DateTime? endDate { get; set; }
    }
}

