using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GalaxyGuesserCLI.DTO
{
    public class SessionQuestionView
    {
        public int sessionId { get; set; }
        public string sessionCode { get; set; } = string.Empty;

        public int questionId { get; set; }

        public string questionText { get; set; } = string.Empty;

        public int categoryId { get; set; }

        public string categoryName { get; set; } = string.Empty;

        public int correctAnswerId { get; set; }

        public List<Option> options { get; set; } = new List<Option>();
    }

    public class Option
    {
        public int optionId { get; set; }
        public string optionText { get; set; } = string.Empty;
        public int answerId { get; set; }
    }
}
