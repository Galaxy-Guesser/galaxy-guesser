namespace GalaxyGuesserApi.Models
{
    public class PlayerDTO
    {
        public int player_id { get; set; }
        public required string username { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

