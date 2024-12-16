namespace PetFeast_Backend2.Models.UserModels.DTOs
{
    public class UserResDTO
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public bool IsBlocked { get; set; }
    }
}
