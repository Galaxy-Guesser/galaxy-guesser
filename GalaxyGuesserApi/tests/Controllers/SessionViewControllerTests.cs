using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GalaxyGuesserApi.Controllers
{
    public class SessionViewControllerTests
    {
        private readonly Mock<ISessionViewService> _mockService;
        private readonly SessionViewController _controller;

        public SessionViewControllerTests()
        {
            _mockService = new Mock<ISessionViewService>();
            _controller = new SessionViewController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllActiveSessions_ReturnsOkWithSessions()
        {
            var testSessions = new List<SessionView>
            {
                new SessionView { sessionId = 1 },
                new SessionView { sessionId = 2}
            };
            
            _mockService.Setup(x => x.GetAllActiveSessions())
                .ReturnsAsync(testSessions);

            var result = await _controller.GetAllActiveSessions();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<SessionView>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedSessions = Assert.IsType<List<SessionView>>(okResult.Value);
            Assert.Equal(2, returnedSessions.Count);
        }

        [Fact]
        public async Task GetAllActiveSessions_WhenExceptionThrown_Returns500()
        {
            _mockService.Setup(x => x.GetAllActiveSessions())
                .ThrowsAsync(new Exception("Test error"));

            var result = await _controller.GetAllActiveSessions();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<SessionView>>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Contains("Test error", statusCodeResult.Value.ToString());
        }
    }
}