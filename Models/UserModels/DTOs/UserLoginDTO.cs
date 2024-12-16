using System.ComponentModel.DataAnnotations;

namespace PetFeast_Backend2.Models.UserModels.DTOs
{
    public class UserLoginDTO
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
