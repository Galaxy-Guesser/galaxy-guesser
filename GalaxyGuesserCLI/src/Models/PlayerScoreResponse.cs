 namespace GalaxyGuesserCLI.Models
{
    public readonly struct PlayerScoreResponse
    {
        public int SessionId { get; init; }
        public int Points { get; init; }

    }

    public record ScoreUpdateResponse(
            bool Success,
            int NewTotalScore,
            string Message
        );
}