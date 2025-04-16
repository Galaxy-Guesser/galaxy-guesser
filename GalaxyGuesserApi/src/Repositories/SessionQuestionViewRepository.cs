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
                SessionCode=  reader.GetString(1),                
                QuestionId = reader.GetInt32(2),            
                QuestionText = reader.GetString(3),                   
                CategoryId = reader.GetInt32(4),               
                CategoryName = reader.GetString(5),               
                CorrectAnswerId = reader.GetInt32(6),         
                Options = JsonConvert.DeserializeObject<List<Option>>(
                    reader.GetString(7))                     
            }, parameters);
        }
    }
}
