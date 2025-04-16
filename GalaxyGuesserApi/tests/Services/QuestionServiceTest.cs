using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyGuesserApi.Services;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;

namespace GalaxyGuesserApi.Tests.Services
{
	public class QuestionServiceTests
	{
		private readonly Mock<IQuestionRepository> _mockRepo;
		private readonly QuestionService _service;

		public QuestionServiceTests()
		{
			_mockRepo = new Mock<IQuestionRepository>();
			_service = new QuestionService(_mockRepo.Object);
		}

		[Fact]
		public async Task GetNextQuestionForSessionAsync_ReturnsQuestionResponse()
		{
			var sessionId = 1;
			var expected = new QuestionResponse
			{
				QuestionId = 123,
				QuestionText = "What is the capital of Mars?"
			};

			_mockRepo.Setup(r => r.GetNextQuestionForSessionAsync(sessionId))
							 .ReturnsAsync(expected);

			var result = await _service.GetNextQuestionForSessionAsync(sessionId);

			Assert.Equal(expected.QuestionId, result.QuestionId);
			Assert.Equal(expected.QuestionText, result.QuestionText);
			_mockRepo.Verify(r => r.GetNextQuestionForSessionAsync(sessionId), Times.Once);
		}

		[Fact]
		public async Task GetOptionsByQuestionIdAsync_ReturnsOptionList()
		{
			int questionId = 123;
			var expected = new List<OptionResponse>
				{
						new OptionResponse { AnswerId = 1, Text = "Option A" },
						new OptionResponse { AnswerId = 2, Text = "Option B" }
				};

			_mockRepo.Setup(r => r.GetOptionsByQuestionIdAsync(questionId))
							 .ReturnsAsync(expected);

			var result = await _service.GetOptionsByQuestionIdAsync(questionId);

			Assert.Equal(2, result.Count);
			Assert.Contains(result, o => o.Text == "Option A");
			_mockRepo.Verify(r => r.GetOptionsByQuestionIdAsync(questionId), Times.Once);
		}

		[Fact]
		public async Task GetCorrectAnswerAsync_ReturnsCorrectAnswer()
		{
			int questionId = 321;
			var expected = new AnswerResponse
			{
				AnswerId = 99,
				Text = "Correct Answer"
			};

			_mockRepo.Setup(r => r.GetCorrectAnswerAsync(questionId))
							 .ReturnsAsync(expected);

			var result = await _service.GetCorrectAnswerAsync(questionId);

			Assert.Equal(expected.AnswerId, result.AnswerId);
			Assert.Equal(expected.Text, result.Text);
			_mockRepo.Verify(r => r.GetCorrectAnswerAsync(questionId), Times.Once);
		}
	}
}