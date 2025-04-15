using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GalaxyGuesserApi.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _icategoryRepository;

        public CategoryService(ICategoryRepository icategoryRepository)
        {
            _icategoryRepository = icategoryRepository;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await _icategoryRepository.GetCategoriesAsync();
        }

    }
}
