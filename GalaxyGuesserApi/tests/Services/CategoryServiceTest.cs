using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;
using GalaxyGuesserApi.Services;
using Moq;
using Xunit;

namespace GalaxyGuesserApi.Tests.Services
{
  public class CategoryServiceTests
  {
    private readonly Mock<ICategoryRepository> _categoryRepoMock;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
      _categoryRepoMock = new Mock<ICategoryRepository>();
      _categoryService = new CategoryService(_categoryRepoMock.Object);
    }

    [Fact]
    public async Task GetAllCategories_ReturnsListOfCategories()
    {
      var mockCategories = new List<Category>
            {
                new Category { categoryId = 1, category = "Science" },
                new Category { categoryId = 2, category = "History" }
            };

      _categoryRepoMock
          .Setup(repo => repo.GetCategoriesAsync())
          .ReturnsAsync(mockCategories);

      var result = await _categoryService.GetAllCategories();

      Assert.NotNull(result);
      Assert.Equal(2, result.Count);
      Assert.Equal("Science", result[0].category);
    }

    [Fact]
    public async Task GetAllCategories_ReturnsEmptyList_WhenNoCategoriesExist()
    {
      _categoryRepoMock
          .Setup(repo => repo.GetCategoriesAsync())
          .ReturnsAsync(new List<Category>());

      var result = await _categoryService.GetAllCategories();

      Assert.NotNull(result);
      Assert.Empty(result);
    }
  }
}