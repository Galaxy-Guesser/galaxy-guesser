using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalaxyGuesserApi.Models
{
    public class SessionScore
    {
        public int player_id { get; set; }
        public int session_id { get; set; }
        public int score { get; set; }

        public record ScoreUpdateRequest(
        int PlayerId,
        int SessionId,
        int QuestionId,
        int AnswerId,
        double SecondsRemaining);

        public record ScoreUpdateResponse(
            bool Success,
            bool IsCorrect = false,
            int BasePoints = 0,
            double TimeBonus = 0,
            int TotalPointsAdded = 0,
            int? NewTotalScore = null,
            double SecondsRemaining = 0,
            string? Message = null);

        public record FinalScoreResponse(
            bool Success,
            int PlayerId,
            int SessionId,
            int TotalScore,
            string? Message = null);

        public record QuestionResult(
            int QuestionId,
            bool IsCorrect,
            double TimeTaken,
            int PointsEarned);

        public record SessionStats(
            int TotalScore);
    }
}