namespace PetFeast_Backend2.Models.CartModels.DTOs
{
    public class CartApiResDTO
    {
        public decimal TotalPrice { get; set; }
        public int TotalCount { get; set; }
        public List<CartResDTO> CartProducts { get; set; }
    }
}
