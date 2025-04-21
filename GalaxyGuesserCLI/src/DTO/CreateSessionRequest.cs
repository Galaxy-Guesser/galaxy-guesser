public class CreateSessionRequest
{
    public int categoryId { get; set; }
    public int questionsCount { get; set; }
    public string userGuid { get; set; } = string.Empty;
    public string startDate { get; set; } = string.Empty;
   public decimal sessionDuration { get; set; }
}
