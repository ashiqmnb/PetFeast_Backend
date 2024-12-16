using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFeast_Backend2.ApiResponce;
using PetFeast_Backend2.Models.ProductModels.DTOs;
using PetFeast_Backend2.Services.ProductService;
using System.Drawing;

namespace PetFeast_Backend2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }



        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productService.GetProducts();
                return Ok(new ApiResponse<List<ProductOutDTO>>(200, "Success", products, null));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                if (product != null)
                {
                    return Ok(new ApiResponse<ProductOutDTO>(200, "Success", product, null));
                }
                return NotFound(new ApiResponse<string>(404, "Failed", null, "Product not found with this id"));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }


        [HttpGet("productByCategory")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProductByCategory(int categoryId)
        {
            try
            {
                var products = await _productService.GetProductByCategory(categoryId);
                if (products != null)
                {
                    return Ok(new ApiResponse<List<ProductOutDTO>>(200, "Success", products, null));
                }
                return NotFound(new ApiResponse<string>(404, "Failed", null, "Product not found with this category"));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            } 
        }



        [HttpGet("ProductPagination")]
        public async Task<IActionResult> ProductPagination([FromQuery] int pageNumber = 1, [FromQuery] int size = 10)
        {
            try
            {
                var products = await _productService.ProductPagination(pageNumber, size);
                return Ok(new ApiResponse<List<ProductOutDTO>>(200, "Success", products, null));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }



        [HttpGet("SearchProduct")]
        public async Task<IActionResult> SearchProduct(string search)
        {
            try
            {
                var products = await _productService.SearchProduct(search);
                return Ok(new ApiResponse<List<ProductOutDTO>>(200, "Success", products, null));

            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }





        [HttpPost("AddProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromForm] ProductCreateDTO newProduct, IFormFile image)
        {
            try
            {
                if (!image.ContentType.StartsWith("image/"))
                    return BadRequest(new ApiResponse<string>(400, "Failed", "Invalid file type. Only images are allowed.", null));

                if (image.Length > 5 * 1024 * 1024) // 5 MB limit
                    return BadRequest(new ApiResponse<string>(400, "Failed", "File size exceeds the 5 MB limit.", null));

                var res = await _productService.AddProduct(newProduct, image);
                if (res == true) return Ok(new ApiResponse<string>(200, "Success", "Product added successfully", null));
                return BadRequest(new ApiResponse<string>(400, "Failed", "Error occured while creating a new product", null));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }



        [HttpPut("UpdateProduct/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductCreateDTO updatedProduct, IFormFile image)
        {
            try
            {
                if (updatedProduct == null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "Invalid product data"));

                var res = await _productService.UpdateProduct(id, updatedProduct, image);
                if (res) return Ok(new ApiResponse<string>(200, "Product updated successfully"));
                return NotFound(new ApiResponse<string>(404, "Product not found"));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
                //return StatusCode(500, new ApiResponse<string>(500, "An error occurred", null, ex.Message));

            }
        }



        // new product update API
        [HttpPut("UpdateProduct2/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] productUpdateDTO updatedProduct, IFormFile image = null)
        {
            try
            {
                if (updatedProduct == null) return BadRequest(new ApiResponse<string>(400, "Failed", null, "Invalid product data"));

                var res = await _productService.UpdateProduct2(id, updatedProduct, image);
                return Ok(new ApiResponse<ProductOutDTO>(200, "Product updated successfully", res, null));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
                //return StatusCode(500, new ApiResponse<string>(500, "An error occurred", null, ex.Message));
            }
        }


        [HttpDelete("DeleteProduct/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var res = await _productService.DeleteProduct(id);
                if (res) return Ok(new ApiResponse<string>(200, "Success", "Product remover succesfully"));
                return NotFound(new ApiResponse<string>(404, "Failed", "Product not found", null));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }



        [HttpGet("TopRatedProducts")]
        public async Task<IActionResult> TopRatedProducts()
        {
            try
            {
                var products = await _productService.TopRatedProducts();
                return Ok(new ApiResponse<List<ProductOutDTO>>(200, "Success", products, null));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
            
    }
}
