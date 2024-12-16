namespace PetFeast_Backend2.Models.WishListModels.DTOs
{
    public class WishListCreateDTO
    {
        // DTO for cereatig a new wishList if user didnt have 

        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
