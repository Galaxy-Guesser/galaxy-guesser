using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using GalaxyGuesserApi.Controllers;
using GalaxyGuesserApi.Services;
using GalaxyGuesserApi.Repositories.Interfaces;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace GalaxyGuesserApi.Tests.Controllers
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
      var mockCategories = new List<Category>
            {
                new Category { categoryId = 1, category = "Stars" },
                new Category { categoryId = 2, category = "Planets" }
            };

      _mockRepository.Setup(repo => repo.GetCategoriesAsync())
          .ReturnsAsync(mockCategories);

      var result = await _controller.GetAllCategories();

      var actionResult = Assert.IsType<ActionResult<IEnumerable<Category>>>(result);
      var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
      var returnValue = Assert.IsType<List<Category>>(okResult.Value);
      Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetAllCategories_Returns500_WhenServiceThrowsException()
    {
      _mockRepository.Setup(repo => repo.GetCategoriesAsync())
          .ThrowsAsync(new System.Exception("Test exception"));

      var result = await _controller.GetAllCategories();

      var actionResult = Assert.IsType<ActionResult<IEnumerable<Category>>>(result);
      var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);
      Assert.Equal(500, objectResult.StatusCode);
      Assert.Contains("Test exception", objectResult.Value.ToString());
    }
  }
}