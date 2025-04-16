using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Repositories.Interfaces
{
    public interface ISessionQuestionViewRepository
    {
   
        Task<List<SessionQuestionView>> GetAllSessionQuestions(int sessionId);
    }
}
