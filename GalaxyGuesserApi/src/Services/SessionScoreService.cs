using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;
using static GalaxyGuesserApi.Models.SessionScore;

namespace GalaxyGuesserApi.Services
{
    public class SessionScoreService
    {
        private readonly ISessionScoreRepository _sessionScoreRepository;

        public SessionScoreService(ISessionScoreRepository sessionScoreRepository)
        {
            _sessionScoreRepository = sessionScoreRepository;
        }

        public async Task<ScoreUpdateResponse> UpdateScoreAsync(ScoreUpdateRequest request,int? playerId)
        {
            await _sessionScoreRepository.UpdatePlayerScoreAsync(playerId, request.SessionId, request.Points);

            var updatedScore = await _sessionScoreRepository.GetPlayerScoreAsync(playerId, request.SessionId);

            return new ScoreUpdateResponse(
                Success: true,
                NewTotalScore: updatedScore ?? 0,
                Message: "Score updated successfully!"
            );
        }

        public async Task<FinalScoreResponse> GetFinalScoreAsync(int playerId, int sessionId)
        {
            var score = await _sessionScoreRepository.GetPlayerScoreAsync(playerId, sessionId);
            if (score == null)
            {
                return new FinalScoreResponse(false, playerId, sessionId, 0, "Player not in session or score not found");
            }
            return new FinalScoreResponse(
                Success: true,
                PlayerId: playerId,
                SessionId: sessionId,
                TotalScore: score.Value,
                Message: "Final score retrieved"
            );
        }

        public async Task<SessionLeaderboardResponse> GetSessionLeaderboardAsync(string sessionCode)
        {
            if (string.IsNullOrWhiteSpace(sessionCode))
            {
                return new SessionLeaderboardResponse
                {
                    Success = false,
                    Message = "Session code cannot be empty",
                    SessionCode = sessionCode,
                    Leaderboard = new List<LeaderboardEntry>()
                };
            }

            var leaderboardEntries = await _sessionScoreRepository.GetSessionLeaderboardAsync(sessionCode);
            
            return new SessionLeaderboardResponse
            {
                Success = leaderboardEntries.Count > 0,
                Message = leaderboardEntries.Count > 0 
                    ? $"Retrieved leaderboard for session {sessionCode}" 
                    : "No leaderboard data found for this session",
                SessionCode = sessionCode,
                Leaderboard = leaderboardEntries
            };
        }

        public async Task<GlobalLeaderboardResponse> GetGlobalLeaderboardAsync()
        {
            var leaderboardEntries = await _sessionScoreRepository.GetGlobalLeaderboardAsync();
            
            return new GlobalLeaderboardResponse
            {
                Success = leaderboardEntries.Count > 0,
                Message = leaderboardEntries.Count > 0
                    ? "Retrieved global leaderboard"
                    : "No global leaderboard data found",
                Leaderboard = leaderboardEntries
            };
        }
    }
}
