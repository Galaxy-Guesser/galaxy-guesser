using GalaxyGuesserApi.Data;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;
using Newtonsoft.Json;



namespace GalaxyGuesserApi.Repositories
{
    public class SessionQuestionViewRepository : ISessionQuestionViewRepository
    {
         private readonly DatabaseContext _dbContext;

        public SessionQuestionViewRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SessionQuestionView>> GetAllSessionQuestions(string sessionCode)
        {
            const string sql = "SELECT * FROM session_questions_view WHERE session_code = @SessionCode";
            var parameters = new Dictionary<string, object>
            {
                { "@SessionCode", sessionCode },
            };

            return await _dbContext.QueryAsync(sql, reader => new SessionQuestionView
            {
                SessionId = reader.GetInt32(0),                
                QuestionId = reader.GetInt32(1),            
                QuestionText = reader.GetString(2),                   
                CategoryId = reader.GetInt32(3),               
                CategoryName = reader.GetString(4),               
                CorrectAnswerId = reader.GetInt32(5),         
                Options = JsonConvert.DeserializeObject<List<Option>>(
                    reader.GetString(6))                     
            }, parameters);
        }
    }
}
