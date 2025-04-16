using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using GalaxyGuesserApi.Controllers;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Services.Interfaces;

namespace GalaxyGuesserApi.Tests
{
    public class QuestionsControllerTests
    {
       private readonly Mock<IQuestionService> _mockQuestionService;
        private readonly QuestionsController _controller;

        public QuestionsControllerTests()
        {
            _mockQuestionService = new Mock<IQuestionService>();
            _controller = new QuestionsController(_mockQuestionService.Object);
        }


    }
}