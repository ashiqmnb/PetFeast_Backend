using PetFeast_Backend2.Models.CategoryModels.DTOs;

namespace PetFeast_Backend2.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<bool> CreateCategory(CategoryCreateDTO category);
        public Task<List<CategoryResDTO>> GetAllCategories();
        public Task<CategoryResDTO> GetCategoryById(int id);
    }
}
