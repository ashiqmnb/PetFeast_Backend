using PetFeast_Backend2.Models.CartModels;
using PetFeast_Backend2.Models.CartModels.DTOs;

namespace PetFeast_Backend2.Services.CartService
{
    public interface ICartService
    {
        Task<CartApiResDTO> GetCartItems(int userId);
        Task<bool> AddToCart(int userId, int productId);
        Task<bool> RemoveFromCart(int userId, int productId);
        Task<bool> IncreaseQuantity(int userId, int productId);
        Task<bool> DecreaseQuantity(int userId, int productId);
        Task<bool> RemoveAllItems(int userId);

    }
}
