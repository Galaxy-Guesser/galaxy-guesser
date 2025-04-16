using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using GalaxyGuesserApi.Controllers;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyGuesserApi.Repositories.Interfaces;

namespace GalaxyGuesserApi.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryRepository> _mockRepository;
        private readonly CategoryService _categoryService;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _mockRepository = new Mock<ICategoryRepository>();
            _categoryService = new CategoryService(_mockRepository.Object);
            _controller = new CategoryController(_categoryService);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsOkResult_WithListOfCategories()
        {
            // Arrange
            var mockCategories = new List<Category>
            {
                new Category { categoryId = 1, category = "Stars" },
                new Category { categoryId = 2, category = "Planets" }
            };
            
            _mockRepository.Setup(repo => repo.GetCategoriesAsync())
                .ReturnsAsync(mockCategories);

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Category>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<List<Category>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAllCategories_Returns500_WhenServiceThrowsException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetCategoriesAsync())
                .ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Category>>>(result);
            var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Contains("Test exception", objectResult.Value.ToString());
        }
    }
}