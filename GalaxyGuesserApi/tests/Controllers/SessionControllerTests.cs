using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GalaxyGuesserApi.Controllers;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GalaxyGuesserApi.tests.Controllers
{
    public class SessionControllerTests
    {
        private readonly Mock<ISessionService> _mockSessionService;
        private readonly SessionsController _controller;

        public SessionControllerTests()
        {
            _mockSessionService = new Mock<ISessionService>();
            _controller = new SessionsController(_mockSessionService.Object);
            
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("sub", "test-user-id"),
                new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
            }, "test"));
            
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Fact]
        public async Task GetSession_WhenExceptionThrown_Returns500()
        {
            // Arrange
            var testCode = "TEST123";
            _mockSessionService.Setup(x => x.GetSessionAsync(testCode))
                .ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _controller.GetSession(testCode);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Session>>(result);
            var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task CreateSession_WithValidRequest_ReturnsOk()
        {
           var request = new CreateSessionRequestDTO 
            { 
                category = "Stars"
            };
            
            _mockSessionService.Setup(x => x.CreateSessionAsync(
                It.IsAny<CreateSessionRequestDTO>(), 
                It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateSession(request);

            Assert.IsAssignableFrom<ActionResult<string>>(result);
            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            
            Assert.Equal("created", okResult.Value);
        }

        [Fact]
        public async Task GetSessions_ReturnsAllSessions()
        {
            // Arrange
            var expectedSessions = new List<SessionDTO>
            {
                new SessionDTO { userName = "Player1" , category = "Stars" }, 
                new SessionDTO { userName = "Player2" , category = "Planet"}  
            };

            _mockSessionService.Setup(x => x.GetAllSessionsAsync())
                .ReturnsAsync(expectedSessions);

            // Act
            var result = await _controller.GetSessions();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<SessionDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedSessions = Assert.IsType<List<SessionDTO>>(okResult.Value);
            Assert.Equal(2, returnedSessions.Count);
        }

        [Fact]
        public async Task GetSessions_WhenExceptionThrown_Returns500()
        {
            // Arrange
            _mockSessionService.Setup(x => x.GetAllSessionsAsync())
                .ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _controller.GetSessions();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<SessionDTO>>>(result);
            var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task JoinSession_WithValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new JoinSessionRequest { sessionCode = "TEST123" };
            _mockSessionService.Setup(x => x.JoinSessionAsync(request.sessionCode, "test-user-id"))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.JoinSession(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Player successfully joined the session.", okResult.Value);
        }
    }
}