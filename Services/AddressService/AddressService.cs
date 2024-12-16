using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetFeast_Backend2.Models.AddressModels;
using PetFeast_Backend2.Models.AddressModels.DTOs;
using PetFeast_Backend2.Models.UserModels.DTOs;

namespace PetFeast_Backend2.Services.AddressService
{
    public class AddressService : IAddressService
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AddressService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<bool> AddAddress(AddressCreateDTO newAddress, int userId)
        {
            try
            {
                if (userId == 0) throw new Exception("User id is invalid");
                if(newAddress == null) throw new Exception("Address cannot to be null");

                var userAddress = await _context.Addresses
                    .Where(a => a.UserId == userId)
                    .ToListAsync();

                if (userAddress.Count() == 3) throw new Exception("Maximum address limit reached");

                var address = new Address
                {
                    UserId = userId,
                    FullName = newAddress.FullName,
                    PhoneNumber = newAddress.PhoneNumber,
                    Pincode = newAddress.Pincode,
                    HouseName = newAddress.HouseName,
                    Place = newAddress.Place,
                    PostOffice = newAddress.PostOffice,
                    LandMark = newAddress.LandMark,
                };
                await _context.Addresses.AddAsync(address);
                await _context.SaveChangesAsync();

                return true;
            }   
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<List<AddressResDTO>> GetAddress(int userId)
        {
            try
            {
                if (userId == 0) throw new Exception("User id is invalid");

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null) throw new Exception("User not found");

                var addresses = await _context.Addresses
                    .Where(a => a.UserId == userId)
                    .ToListAsync();

                if (addresses != null)
                {
                    return _mapper.Map<List<AddressResDTO>>(addresses);
                }

                return new List<AddressResDTO>();
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> RemoveAddress(int addressId, int userId)
        {
            try
            {
                if(userId == 0 || addressId == 0) throw new Exception("User id or addrss id is invalid");

                var address = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.UserId == userId && a.AddressId == addressId);

                if (address != null)
                {
                    _context.Addresses.Remove(address);
                    await _context.SaveChangesAsync();

                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
