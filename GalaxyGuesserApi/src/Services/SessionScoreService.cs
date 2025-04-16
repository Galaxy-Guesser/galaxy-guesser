using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ScoreUpdateResponse> UpdateScoreAsync(ScoreUpdateRequest request)
        {
            await _sessionScoreRepository.UpdatePlayerScoreAsync(request.PlayerId, request.SessionId, request.Points);

            var updatedScore = await _sessionScoreRepository.GetPlayerScoreAsync(request.PlayerId, request.SessionId);

            return new ScoreUpdateResponse(
                Success: true,
                IsCorrect: true,
                basePoints,
                timeBonus,
                totalPoints,
                updatedScore,
                request.SecondsRemaining,
                "Answer correct!");
        }

        public async Task<FinalScoreResponse> GetFinalScoreAsync(int playerId, int sessionId)
        {
            if (!await _sessionScoreRepository.IsPlayerInSessionAsync(playerId, sessionId))
                return new FinalScoreResponse(false, playerId, sessionId, 0, "Player not in session");

            var score = await _sessionScoreRepository.GetPlayerScoreAsync(playerId, sessionId);
            return new FinalScoreResponse(
                true,
                playerId,
                sessionId,
                score ?? 0,
                "Final score retrieved");
        }
    }
}