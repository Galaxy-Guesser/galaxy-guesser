namespace GalaxyGuesserApi.Models
{
    public class Player
    {
        public int player_id { get; set; }
        public required string username { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

