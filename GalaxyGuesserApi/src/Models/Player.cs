
namespace GalaxyGuesserApi.Models
{
    public readonly struct Player
    {
        public int playerId { get; init; }
        public required string userName { get; init; }
        public required string guid { get; init; }
    }
}

