using PetFeast_Backend2.Models.CartModels;
using PetFeast_Backend2.Models.CategoryModels;
using System.ComponentModel.DataAnnotations;

namespace PetFeast_Backend2.Models.ProductModels
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Product description is required")]
        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }

        //[Required(ErrorMessage = "Image URL is required")]
        //[Url(ErrorMessage = "Invalid URL format")]
        public string? Image { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        public decimal Rating { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        public int? CategoryId { get; set; }


        // newly added


        [Required(ErrorMessage = "Stock is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Stock must be greater than 0")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "MRP is required")]
        [Range(0, double.MaxValue, ErrorMessage = "MRP must be greater than 0")]
        public decimal MRP { get; set; }


        // Navigation property to represent the category and cartItems associated with this product
        public virtual Category? Category { get; set; }
        public virtual List<CartItem>? CartItems { get; set; }



    }
}
