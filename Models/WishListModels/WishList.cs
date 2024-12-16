using PetFeast_Backend2.Models.ProductModels;
using PetFeast_Backend2.Models.UserModels;

namespace PetFeast_Backend2.Models.WishListModels
{
    public class WishList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }

        public virtual User? User { get; set; }
        public virtual Product? Product { get; set; }

    }
}
