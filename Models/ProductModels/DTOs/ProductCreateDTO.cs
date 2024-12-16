using System.ComponentModel.DataAnnotations;

namespace PetFeast_Backend2.Models.ProductModels.DTOs
{
    public class ProductCreateDTO
    {
        [Required(ErrorMessage = "Product name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Product description is required")]
        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Offer price must be greater than or equal to 0")]
        public decimal Price { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public decimal Rating { get; set; }

        [Required(ErrorMessage = "Category ID is required.")]
        public int CategoryId { get; set; }

        // newly added

        [Required(ErrorMessage = "Stock is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Stock must be greater than 0")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "MRP is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "MRP must be greater than 0")]
        public decimal MRP { get; set; }

    }
}
