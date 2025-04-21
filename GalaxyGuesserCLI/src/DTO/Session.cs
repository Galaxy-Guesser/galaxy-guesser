 namespace GalaxyGuesserCLI.DTO
 {
    class Session
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int CategoryId { get; set; }
        public DateTime StartDate { get; set; }
        public int QuestionDuration { get; set; }
        public int QuestionCount { get; set; } 

        public Session(int id, string code, int categoryId, int questionDuration, int questionCount)
        {
            Id = id;
            Code = code;
            CategoryId = categoryId;
            StartDate = DateTime.Now;
            QuestionDuration = questionDuration;
            QuestionCount = questionCount;
        }
    }
 }
