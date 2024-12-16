using PetFeast_Backend2.Models.OrderModels.DTOs;

namespace PetFeast_Backend2.Services.OrderService
{
    public interface IOrderService
    { 

        Task<string> RazorOrderCreate(long price);
        bool RazorPayment(PaymentDTO payment);
        Task<bool> CreateOrder(int userId, CreateOrderDTO createOrderDto);
        Task<List<OrderUserDetailViewDTO>> GetOrderDetails(int userId);
        Task<List<OrderAdminViewDTO>> GetOrderDetailsAdmin();
        Task<List<OrderUserDetailViewDTO>> GetOrdersByUserId(int userId);
        Task<decimal> TotalRevenue();
        Task<int> TotalProductsPurchased();
        Task<bool> UpdateOrderStatus(int orderId, string newStatus);
        Task<bool> CancelOrder(int userId, int orderId);
    }
}
