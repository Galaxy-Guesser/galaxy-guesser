public class CreateSessionRequest
{
    public int categoryId { get; set; }
    public int questionsCount { get; set; }
    public string userGuid { get; set; }
    public string startDate { get; set; }
   public decimal sessionDuration { get; set; }
}
