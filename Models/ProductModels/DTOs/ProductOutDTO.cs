using System.ComponentModel.DataAnnotations;

namespace PetFeast_Backend2.Models.ProductModels.DTOs
{
    public class ProductOutDTO
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }   
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public decimal Rating { get; set; }

        // Newly added
        public int Stock { get; set; }
        public decimal MRP { get; set; }
    }
}
