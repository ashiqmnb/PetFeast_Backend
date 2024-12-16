using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFeast_Backend2.ApiResponce;
using PetFeast_Backend2.Models.UserModels.DTOs;
using PetFeast_Backend2.Services.UserService;

namespace PetFeast_Backend2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetUsers")]
        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> GetUsers()
        {
            try
            {
                var users = await _userService.GetUsers();
                return Ok(new ApiResponse<List<UserResDTO>>(200, "Success", users, null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }


        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                var user = await _userService.GetUserById(userId);
                if(user == null) return NotFound(new ApiResponse<string>(404, "Failed", null, "Useer not found"));
                return Ok(new ApiResponse<UserResDTO>(200, "Success", user, null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }


        [HttpPatch("BlockOrUnblock/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockOrUnblock(int userId)
        {
            try
            {
                var res = await _userService.BlockOrUnblock(userId);
                return Ok(new ApiResponse<BlockUnblockRes>(200, "Success", res, null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
    }
}
