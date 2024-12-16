using PetFeast_Backend2.Models.AddressModels.DTOs;

namespace PetFeast_Backend2.Services.AddressService
{
    public interface IAddressService
    {
        Task<bool> AddAddress(AddressCreateDTO newAddress, int userId);
        Task<List<AddressResDTO>> GetAddress(int userId);
        Task<bool> RemoveAddress(int addressId, int userId);
    }
}
