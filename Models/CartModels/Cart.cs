using PetFeast_Backend2.Models.UserModels;

namespace PetFeast_Backend2.Models.CartModels
{
    public class Cart
    {
        public int CartId { get; set; }

        public int UserId { get; set; }

        // Navigation property to represent the user associated with this cart
        public virtual User User { get; set; }

        public virtual List<CartItem> cartItems { get; set; }
    }
}
