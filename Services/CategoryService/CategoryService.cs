using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetFeast_Backend2.Models.CategoryModels;
using PetFeast_Backend2.Models.CategoryModels.DTOs;

namespace PetFeast_Backend2.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(IConfiguration config, AppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> CreateCategory(CategoryCreateDTO category)
        {
            try
            {
                var newCategory = _mapper.Map<Category>(category);
                await _context.Categories.AddAsync(newCategory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error creating category: {ex.Message}");
                return false;
            }
        }

        public async Task<List<CategoryResDTO>> GetAllCategories()
        {
            try
            {
                var allCategories = await _context.Categories.ToListAsync();
                List<CategoryResDTO> categories = _mapper.Map<List<CategoryResDTO>>(allCategories);
                return categories;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retreiving category: {ex.Message}");
                return null;
            }
        }


        public async Task<CategoryResDTO> GetCategoryById(int categoryId)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if(category != null)
                {
                    return _mapper.Map<CategoryResDTO>(category);
                }
                return null;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retreiving category: {ex.Message}");
                return null;
            }
        }
    }
}
