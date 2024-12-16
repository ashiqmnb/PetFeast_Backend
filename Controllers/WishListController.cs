using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFeast_Backend2.ApiResponce;
using PetFeast_Backend2.Models.WishListModels.DTOs;
using PetFeast_Backend2.Services.WishListService;
using System.Security.Claims;

namespace PetFeast_Backend2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {

        private readonly IWishListService _wishListService;

        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }


        [HttpGet("GetWishList")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetWishList()
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                var wishLists = await _wishListService.GetWishList(userId);
                return Ok(new ApiResponse<List<WishListResDTO>>(200, "Success", wishLists, null));
            }
            catch(Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }



        [HttpPost("AddOrRemove")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddOrRemove(int productId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                var res = await _wishListService.AddOrRemove(userId, productId);
                return Ok(new ApiResponse<string>(200, "Success", res, null));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }


        [HttpGet("checkInWishlist")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> checkInWishlist(int productId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                bool res = await _wishListService.checkInWishlist(productId, userId);
                if(res)
                {
                    return Ok(new ApiResponse<bool>(200, "Success", true, null));
                }
                return Ok(new ApiResponse<bool>(200, "Success", false, null));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
    } 
}
