using System.ComponentModel.DataAnnotations;

namespace PetFeast_Backend2.Models.CategoryModels.DTOs
{
    public class CategoryResDTO
    {
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
