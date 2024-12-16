using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetFeast_Backend2.Models.UserModels.DTOs;

namespace PetFeast_Backend2.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public UserService(IMapper mapper, AppDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserResDTO>> GetUsers()
        {
            try
            {
                var users = await _context.Users
                    .Where(u => u.Role == "User")
                    .ToListAsync();
                return _mapper.Map<List<UserResDTO>>(users);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<UserResDTO> GetUserById(int userId)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.Role == "User")
                    .FirstOrDefaultAsync(u => u.UserId == userId);
                if (user == null) throw new Exception("User not found");
                return _mapper.Map<UserResDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<BlockUnblockRes> BlockOrUnblock(int userId)
        {
            try
            {
                var user = _context.Users
                    .Where(u => u.Role == "User")
                    .FirstOrDefault(u => u.UserId == userId);
                if (user == null) throw new Exception("User not found");

                user.IsBlocked = !user.IsBlocked;
                await _context.SaveChangesAsync();
                return new BlockUnblockRes()
                {
                    IsBlocked = user.IsBlocked,
                    Message = user.IsBlocked == true ? "User is blocked" : "User is unblocked"
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
