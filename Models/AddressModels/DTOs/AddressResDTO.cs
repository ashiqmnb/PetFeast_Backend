using System.ComponentModel.DataAnnotations;

namespace PetFeast_Backend2.Models.AddressModels.DTOs
{
    public class AddressResDTO
    {
        public int AddressId { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Pincode { get; set; }
        public string? HouseName { get; set; }
        public string? Place { get; set; }
        public string? PostOffice { get; set; }
        public string? LandMark { get; set; }
    }
}
