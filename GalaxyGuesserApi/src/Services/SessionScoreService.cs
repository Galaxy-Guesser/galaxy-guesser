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
            // Validate player is in session
            if (!await _sessionScoreRepository.IsPlayerInSessionAsync(request.PlayerId, request.SessionId))
            {
                return new ScoreUpdateResponse(false, Message: "Player is not in this session");
            }

            // Validate question is in session
            if (!await _sessionScoreRepository.IsQuestionInSessionAsync(request.QuestionId, request.SessionId))
            {
                return new ScoreUpdateResponse(false, Message: "Question not in this session");
            }

            // Check if answer is correct
            var correctAnswerId = await _sessionScoreRepository.GetCorrectAnswerIdAsync(request.QuestionId);
            if (request.AnswerId != correctAnswerId)
            {
                return new ScoreUpdateResponse(
                    Success: false,
                    IsCorrect: false,
                    Message: "Incorrect answer");
            }

            // Calculate points
            int basePoints = 1;
            double timeBonus = Math.Round((request.SecondsRemaining / 30.0) * 5, 2);
            int totalPoints = basePoints + (int)timeBonus;

            // Update score
            var currentScore = await _sessionScoreRepository.GetPlayerScoreAsync(request.PlayerId, request.SessionId);
            if (currentScore.HasValue)
            {
                await _sessionScoreRepository.UpdatePlayerScoreAsync(request.PlayerId, request.SessionId, totalPoints);
            }
            else
            {
                await _sessionScoreRepository.AddPlayerScoreAsync(request.PlayerId, request.SessionId, totalPoints);
            }

            // Get updated score
            var updatedScore = await _sessionScoreRepository.GetPlayerScoreAsync(request.PlayerId, request.SessionId);

            return new ScoreUpdateResponse(
                Success: true,
                IsCorrect: true,
                basePoints,
                timeBonus,
                totalPoints,
                updatedScore,
                request.SecondsRemaining,
                "Answer correct! Points added.");
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