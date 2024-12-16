using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFeast_Backend2.ApiResponce;
using PetFeast_Backend2.Migrations;
using PetFeast_Backend2.Models.OrderModels.DTOs;
using PetFeast_Backend2.Services.OrderService;
using Razorpay.Api;
using System.Collections.Generic;
using System.Security.Claims;

namespace PetFeast_Backend2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpPost("CreateOrder")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateOrder(long price)
        {
            try
            {
                if(price <= 0)
                {
                    return BadRequest(new ApiResponse<string>(400, "Failed", null, "Price is invalid"));
                }

                var orderId = await _orderService.RazorOrderCreate(price);
                return Ok(new ApiResponse<string>(200, "Success", orderId));
            }
            catch (Exception ex)
            { 
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }


        [HttpPost("Payment")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Payment(PaymentDTO razorPay)
        {
            try
            {
                if(razorPay == null)
                {
                    return BadRequest(new ApiResponse<string>(400, "Failed", null, "Razorpay details should not to be null"));
                }

                var res =   _orderService.RazorPayment(razorPay);
                if(res == false)
                {
                    return BadRequest(new ApiResponse<string>(400, "Failed", null, "Razorpay details should not to be null"));
                }
                return Ok(new ApiResponse<string>(200, "Seccess", "Payment completed successfully", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }


        [HttpPost("PlaceOrder")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> PlaceOrder(CreateOrderDTO orderCreate)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);

                var res = await _orderService.CreateOrder(userId, orderCreate);
                if(res == false)
                {
                    return BadRequest(new ApiResponse<string>(400, "Failed", null, "Order placing failed"));
                }
                return Ok(new ApiResponse<string>(200, "Success", "Order placed successfully", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }


        [HttpGet("GetOrderDetails")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetOrderDetails()
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var orderDetails = await _orderService.GetOrderDetails(userId);
                return Ok(new ApiResponse<List<OrderUserDetailViewDTO>>(200, "Success", orderDetails, null));

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }


        [HttpGet("GetOrdersByUserId")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            try
            {
                var orderDetails = await _orderService.GetOrdersByUserId(userId);
                if(orderDetails == null)
                {
                    return NotFound(new ApiResponse<string>(404, "Failed", null, "Order details not found"));
                }
                return Ok(new ApiResponse<List<OrderUserDetailViewDTO>>(200, "Success", orderDetails, null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }


        [HttpGet("GetOrderDetailsAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrderDetailsAdmin()
        {
            try
            {
                var orderDetails = await _orderService.GetOrderDetailsAdmin();
                if(orderDetails == null)
                {
                    return NotFound(new ApiResponse<string>(404, "Failed", null, "Order details not found"));
                }
                return Ok(new ApiResponse<List<OrderAdminViewDTO>>(200, "Success", orderDetails, null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }


        [HttpGet("TotalRevenue")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TotalRevenue()
        {
            try
            {
                var totalRevenue = await _orderService.TotalRevenue();
                return Ok(new ApiResponse<int>(200, "Success", Convert.ToInt32(totalRevenue), null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }


        [HttpGet("TotalProductsPurchased")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TotalProductsPurchased()
        {
            try
            {
                var totalPrdcts = await _orderService.TotalProductsPurchased();
                return Ok(new ApiResponse<int>(200, "Success", totalPrdcts, null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }


        [HttpPatch("UpdateOrderStatus")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string newStatus)
        {
            try
            {
                var res = await _orderService.UpdateOrderStatus(orderId, newStatus);
                if (res == false)
                {
                    return BadRequest(new ApiResponse<string>(400, "Failed", null, "Order status updating failed"));
                }
                return Ok(new ApiResponse<string>(200, "Success", "Order status updating successfully", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }

        [HttpPatch("CancelOrder")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Items["UserId"]);
                var res = await _orderService.CancelOrder(userId, orderId);

                if (res == false)
                {
                    return BadRequest(new ApiResponse<string>(400, "Failed", null, "Order cancelling failed"));
                }
                return Ok(new ApiResponse<string>(200, "Success", "Order cancelled successfully", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, "Failed", null, ex.Message));
            }
        }
    }
}
