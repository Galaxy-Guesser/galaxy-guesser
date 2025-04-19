 namespace GalaxyGuesserCLI.Models
{
    class SessionPlayer
    {
        public int SessionId { get; set; }
        public int PlayerId { get; set; }

        public SessionPlayer(int sessionId, int playerId)
        {
            SessionId = sessionId;
            PlayerId = playerId;
        }
    }
}