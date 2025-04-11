using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GalaxyGuesserApi.Services
{
    public class SessionViewService
    {
        private readonly ISessionViewRepository _sessionViewRepository;

        public SessionViewService(ISessionViewRepository sessionViewRepository)
        {
            _sessionViewRepository = sessionViewRepository;
        }

        public async Task<List<SessionView>> GetAllActiveSessions()
        {
            return await _sessionViewRepository.GetAllActiveSessions();
        }

    }
}
