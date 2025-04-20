public class CreateSessionRequest
{
    public required string category { get; set; }
    public int questionsCount { get; set; }
    public string? userGuid { get; set; }
    public required string startDate { get; set; }
   public decimal sessionDuration { get; set; }
}
