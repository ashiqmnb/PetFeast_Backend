using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFeast_Backend2.ApiResponce;
using PetFeast_Backend2.Models.CategoryModels.DTOs;
using PetFeast_Backend2.Services.Auth;
using PetFeast_Backend2.Services.CategoryService;

namespace PetFeast_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAllCategories")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                List<CategoryResDTO> categories = await _categoryService.GetAllCategories();
                if(categories == null || categories.Count == 0)
                {
                    return NotFound(new ApiResponse<string>(404, "Not Found", null, "no categories found"));
                }
                return Ok(new ApiResponse<List<CategoryResDTO>>(200, "Success", categories, null));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }


        [HttpGet("GetCategoryById")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            try
            {
                if (categoryId <= 0)
                {
                    return BadRequest(new ApiResponse<string>(400, "Failed", null, "Invalid Id"));
                }
                var category = await _categoryService.GetCategoryById(categoryId);
                if (category == null)
                {
                    return NotFound(new ApiResponse<string>(404, "failed", null, "Category with this id is not found"));
                }
                return Ok(new ApiResponse<CategoryResDTO>(200, "Success", category, null));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }


        [HttpPost("CreateCategory")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory(CategoryCreateDTO newCategory)
        {
            try
            {
                var res = await _categoryService.CreateCategory(newCategory);
                if(res == true)
                {
                    return Ok(new ApiResponse<string>(200, "Success", "Category created successfully", null));
                }
                return BadRequest(new ApiResponse<string>(400, "Failed", "Error occured while creating new category", null));
            }
            catch(Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }

    }
}
