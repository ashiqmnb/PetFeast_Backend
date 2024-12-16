using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PetFeast_Backend2.Models.ProductModels;
using System.ComponentModel.DataAnnotations;

namespace PetFeast_Backend2.Models.CartModels
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int CartId { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Navigation properties to represent the cart & Product associated with this cartItem
        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }    
    }
}
