using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ConsoleApp1.Models
{
    public class SessionQuestionView
    {
        public int sessionId { get; set; }
        public required string sessionCode { get; set; }

        public int questionId { get; set; }

        public required string questionText { get; set; }

        public int categoryId { get; set; }

        public required string categoryName { get; set; }

        public int correctAnswerId { get; set; }

        public List<Option> options { get; set; } = new List<Option>();
    }

    public class Option
    {
        public int optionId { get; set; }
        public required string optionText { get; set; }
        public int answerId { get; set; }
    }
}
