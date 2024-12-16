using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFeast_Backend2.ApiResponce;
using PetFeast_Backend2.Models.AddressModels.DTOs;
using PetFeast_Backend2.Services.AddressService;
using PetFeast_Backend2.Services.CartService;

namespace PetFeast_Backend2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IConfiguration _configuration;

        // Constructor to initialize dependencies
        public AddressController(IAddressService addressService, IConfiguration configuration)
        {
            _addressService = addressService;
        }


        [HttpPost("AddAddress")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddAddress(AddressCreateDTO newAddress)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var res = await _addressService.AddAddress(newAddress, userId);

                if(res) return Ok(new ApiResponse<string>(200, "Success", "Address added successfully", null));

                return BadRequest(new ApiResponse<string>(400, "Failed", null, "Error occured while adding address"));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }



        [HttpGet("GetAddress")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAddress()
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                var addresses = await _addressService.GetAddress(userId);

                if (addresses != null) return Ok(new ApiResponse<List<AddressResDTO>>(200, "Success", addresses, null));

                return BadRequest(new ApiResponse<string>(400, "Failed", null, "Error occured while fetching address"));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }



        [HttpDelete("DeleteAddress")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var res = await _addressService.RemoveAddress(addressId, userId);

                if (res) return Ok(new ApiResponse<string>(200, "Success", null, null));

                return BadRequest(new ApiResponse<string>(400, "Failed", null, "Error occured while removing address"));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                //Console.WriteLine("aaaaaaaaaaa",ex.InnerException.Message);
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
    }
}
