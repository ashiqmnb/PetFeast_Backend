using PetFeast_Backend2.Models.AddressModels;
using PetFeast_Backend2.Models.CartModels;
using PetFeast_Backend2.Models.OrderModels;
using PetFeast_Backend2.Models.WishListModels;
using System.ComponentModel.DataAnnotations;

namespace PetFeast_Backend2.Models.UserModels
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage ="User name is required")]
        [StringLength(25,ErrorMessage = "Name should not exceed 25 characters")]
        public string? Name { get; set; } // full name in json

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one letter, one number, and one special character.")]
        public string? Password { get; set; }


        // User Role
        public string? Role {  get; set; }

        //User Blocked or Not
        public bool IsBlocked { get; set; }



        // Navigation properties
        public virtual Cart? Cart { get; set; }
        public virtual List<WishList>? WishLists { get; set; }
        public virtual List<OrderMain>? Orders { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }


    }
}
