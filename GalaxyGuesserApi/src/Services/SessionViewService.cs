using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;
using GalaxyGuesserApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GalaxyGuesserApi.Services
{
    public class SessionViewService: ISessionViewService
    {
        private readonly ISessionViewService _sessionViewService;

        public SessionViewService(ISessionViewService sessionViewService)
        {
            _sessionViewService = sessionViewService;
        }

        public async Task<List<SessionView>> GetAllActiveSessions()
        {
            return await _sessionViewService.GetAllActiveSessions();
        }
    }
}
