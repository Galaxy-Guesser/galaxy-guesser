using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GalaxyGuesserCLI.Models
{
    public class SessionQuestionView
    {
        public int sessionId { get; set; }
        public string sessionCode { get; set; }

        public int questionId { get; set; }

        public string questionText { get; set; }

        public int categoryId { get; set; }

        public string categoryName { get; set; }

        public int correctAnswerId { get; set; }

        public List<Option> options { get; set; } = new List<Option>();
    }

    public class Option
    {
        public int optionId { get; set; }
        public string optionText { get; set; }
        public int answerId { get; set; }
    }
}
