using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFeast_Backend2.ApiResponce;
using PetFeast_Backend2.Models.CartModels.DTOs;
using PetFeast_Backend2.Models.ProductModels;
using PetFeast_Backend2.Services.CartService;
using System.Security.Claims;

namespace PetFeast_Backend2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartServices;
        private readonly IConfiguration _configuration;

        // Constructor to initialize dependencies
        public CartController(ICartService cartServices, IConfiguration configuration)
        {
            _configuration = configuration;
            _cartServices = cartServices;
        }



        [HttpPost("AddToCart")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddToCart(int productId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                var res = await _cartServices.AddToCart(userId, productId);
                if(res == true)
                {
                    return Ok(new ApiResponse<string>(200, "Product added to cart"));
                }
                return BadRequest(new ApiResponse<string>(400,"Failed", null, "Product with this id is not exist "));
                
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }


        [HttpGet("GetCartItems")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetCartItems()
        {
            try
            {

                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                //Console.WriteLine($"UserId: {userId}");

                var cartItems = await _cartServices.GetCartItems(userId);
                return Ok(new ApiResponse<CartApiResDTO>(200, "Success", cartItems));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }



        [HttpDelete("RemoveFromCart")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);


                var res = await _cartServices.RemoveFromCart(userId, productId);
                if (res == true)
                {
                    return Ok(new ApiResponse<string>(200, "Product deleted from cart"));
                }
                return BadRequest(new ApiResponse<string>(400, "Error occure while deleting product from cart"));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }


        [HttpDelete("RemoveAllItems")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RemoveAllItems()
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                var res = await _cartServices.RemoveAllItems(userId);
                if (res)
                {
                    return Ok(new ApiResponse<string>(200, "All items cleared successfully"));
                }
                return BadRequest(new ApiResponse<string>(400, "Failed to clear the cart"));

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }




        [HttpPut("IncreaseQuantity")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> IncreaseQuantity(int productId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                var res = await _cartServices.IncreaseQuantity(userId, productId);
                if (res == true)
                {
                    return Ok(new ApiResponse<string>(200, "Product quantity increased"));
                }
                return BadRequest(new ApiResponse<string>(400, "Error occure while increasing product quantity"));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }



        [HttpPut("DecreaseQuantity")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DecreaseQuantity(int productId)
        {
            try
            {
                //var userId = GetingUserIdByClaims();
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                var res = await _cartServices.DecreaseQuantity(userId, productId);
                if (res == true)
                {
                    return Ok(new ApiResponse<string>(200, "Product quantity decreased"));
                }
                return BadRequest(new ApiResponse<string>(400, "Error occure while decreasing product quantity"));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
    }
}
