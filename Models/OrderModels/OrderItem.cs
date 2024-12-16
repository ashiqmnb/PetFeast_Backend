using PetFeast_Backend2.Models.ProductModels;
using System.ComponentModel.DataAnnotations;

namespace PetFeast_Backend2.Models.OrderModels
{
    public class OrderItem
    {   
        public int Id {  get; set; }

        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Total price is required.")]
        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }


        // Navigation property to represent the order and product associated with this item
        public virtual OrderMain? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
