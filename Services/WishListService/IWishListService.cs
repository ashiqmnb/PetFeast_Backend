using PetFeast_Backend2.Models.WishListModels.DTOs;

namespace PetFeast_Backend2.Services.WishListService
{
    public interface IWishListService
    {
        Task<string> AddOrRemove(int userId, int productId);
        Task<List<WishListResDTO>> GetWishList(int userId);
        Task<bool> checkInWishlist(int producId, int userId);
    }
}
