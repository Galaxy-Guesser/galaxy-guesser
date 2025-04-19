namespace GalaxyGuesserCLI.Models
{ 
    class SessionQuestion
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int QuestionId { get; set; }

        public SessionQuestion(int id, int sessionId, int questionId)
        {
            Id = id;
            SessionId = sessionId;
            QuestionId = questionId;
        }
    }
}