using System;

namespace GalaxyGuesserApi.Models
{
    public class SessionScore
    {
        public int player_id { get; set; }
        public int session_id { get; set; }
        public int score { get; set; }

        public record ScoreUpdateRequest(
            int SessionId,
            int Points
        );

        public record ScoreUpdateResponse(
            bool Success,
            int NewTotalScore,
            string Message
        );

        public record FinalScoreResponse(
            bool Success,
            int PlayerId,
            int SessionId,
            int TotalScore,
            string? Message = null
        );

        public record QuestionResult(
            int QuestionId,
            bool IsCorrect,
            double TimeTaken,
            int PointsEarned
        );

        public record SessionStats(
            int TotalScore
        );
    }
}
