namespace PetFeast_Backend2.Models.WishListModels.DTOs
{
    public class WishListResDTO
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Category { get; set; }
        public string? Image {  get; set; }

        //
        //public decimal MRP { get; set; }

    }
}
