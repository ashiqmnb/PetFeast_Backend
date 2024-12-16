namespace PetFeast_Backend2.Models.CartModels.DTOs
{
    public class CartResDTO
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        
        public string? Image { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }

        // newly addded
        public decimal MRP { get; set; }
        public int Stock {  get; set; }
    }
}
