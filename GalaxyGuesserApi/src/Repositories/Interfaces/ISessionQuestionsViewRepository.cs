using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Repositories.Interfaces
{
    public interface ISessionQuestionViewRepository
    {
   
        Task<List<SessionQuestionView>> GetAllSessionQuestions(string sessionCode);
    }
}
