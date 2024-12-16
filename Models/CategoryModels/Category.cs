using PetFeast_Backend2.Models.ProductModels;
using System.ComponentModel.DataAnnotations;

namespace PetFeast_Backend2.Models.CategoryModels
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        public string? Name { get; set; }

        public virtual ICollection<Product>? Products {  get; set; }
    }
}
