using PetFeast_Backend2.Migrations;
using PetFeast_Backend2.Models.AddressModels;
using PetFeast_Backend2.Models.UserModels;
using System.ComponentModel.DataAnnotations;

namespace PetFeast_Backend2.Models.OrderModels
{
    public class OrderMain
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public int AddressId { get; set; }

        //[Required]
        //public string? CustomerName { get; set; }

        //[Required]
        //public string? CustomerEmail { get; set; }

        //[Required]
        //public string? CustomerPhone { get; set; }

        //[Required]
        //public string? CustomerCity { get; set; }

        //[Required]
        //public string? HomeAddress { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public string? OrderString { get; set; }

        [Required]
        public string? TransactionId { get; set; }

        [Required]
        public string? OrderStatus { get; set; }


        // Navigation property to represent the order and product associated with this item
        public Address? Address { get; set; }
        public User? User { get; set; }
        public List<OrderItem>? OrderItems {  get; set; }
    }
}
