using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetFeast_Backend2.ApiResponce;
using PetFeast_Backend2.Models.UserModels.DTOs;
using PetFeast_Backend2.Services.Auth;

namespace PetFeast_Backend2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDTO userReg)
        {
            try
            {
                string result = await _authService.Register(userReg);
                if(result == null)
                {
                    //var apiRes = new ApiResponse<string>(400, "Registration Failed");
                    return BadRequest(new ApiResponse<string> ( 400, "Registration Failed"));
                }
                return Ok(new ApiResponse<string>(200, "User registered successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Registration Failed", null, ex.Message));
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO userLogin)
        {
            try
            {
                var loginRes =  await _authService.Login(userLogin);
                return Ok(new ApiResponse<UserLoginResDTO>(200, "Login Successfull", loginRes, null));
            }
            catch(Exception ex)
            {
                return Unauthorized(new ApiResponse<string>(401, "Login Failed", null, ex.Message ));
            }
        }
    }
}
