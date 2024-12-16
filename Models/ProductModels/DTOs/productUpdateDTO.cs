using System.ComponentModel.DataAnnotations;

namespace PetFeast_Backend2.Models.ProductModels.DTOs
{
    public class productUpdateDTO
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "MRP must be greater than 0")]
        public decimal Price { get; set; }

        public decimal Rating { get; set; }

        public int CategoryId { get; set; }

        // newly added

        [Range(1, int.MaxValue, ErrorMessage = "Stock must be greater than 0")]
        public int Stock { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "MRP must be greater than 0")]
        public decimal MRP { get; set; }
    }
}
