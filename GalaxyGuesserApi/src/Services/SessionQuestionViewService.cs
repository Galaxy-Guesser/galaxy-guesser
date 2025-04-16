using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GalaxyGuesserApi.Services
{
    public class SessionQuestionsViewService
    {
        private readonly ISessionQuestionViewRepository _sessionQuestionViewRepository;

        public SessionQuestionsViewService(ISessionQuestionViewRepository sessionQuestionViewRepository)
        {
            _sessionQuestionViewRepository = sessionQuestionViewRepository;
        }

        public async Task<List<SessionQuestionView>> GetAllSessionQuestions(int SessionId)
        {
            return await _sessionQuestionViewRepository.GetAllSessionQuestions(SessionId);
        }

    }
}
