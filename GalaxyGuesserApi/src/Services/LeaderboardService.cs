using GalaxyGuesserApi.Repositories.Interfaces;
using GalaxyGuesserApi.Services.Interfaces;
using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly ILeaderboardRepository _leaderboardRepository;

        public LeaderboardService(ILeaderboardRepository leaderboardRepository)
        {
            _leaderboardRepository = leaderboardRepository;
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

            var leaderboardEntries = await _leaderboardRepository.GetSessionLeaderboardAsync(sessionCode);
            
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
            var leaderboardEntries = await _leaderboardRepository.GetGlobalLeaderboardAsync();
            
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