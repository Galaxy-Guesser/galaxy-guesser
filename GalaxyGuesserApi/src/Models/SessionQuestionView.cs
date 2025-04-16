using System;
using System.Collections.Generic;
using System.Text.Json.Serialization; // For JsonPropertyName if using System.Text.Json

public class SessionQuestionView
{
    public int SessionId { get; set; }

    public int QuestionId { get; set; }

    public string QuestionText { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public int CorrectAnswerId { get; set; }

    public List<Option> Options { get; set; } = new List<Option>();

}