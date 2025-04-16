namespace GalaxyGuesserApi.Models
{
  public class QuestionResponse
  {
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
  }

  public class OptionResponse
  {
    public int AnswerId { get; set; }
    public string Text { get; set; } = string.Empty;
  }

  public class AnswerResponse
  {
    public int AnswerId { get; set; }
    public string Text { get; set; } = string.Empty;
  }
}