using System.ComponentModel.DataAnnotations;

namespace PetFeast_Backend2.Models.OrderModels.DTOs
{
    public class OrderAdminViewDTO
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? TransactionId { get; set; }
        public decimal TotalPrice { get; set; }

        //
        public string? Orderstatus { get; set; }
    }
}
