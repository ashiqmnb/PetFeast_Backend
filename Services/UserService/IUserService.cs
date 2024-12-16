using PetFeast_Backend2.Models.UserModels.DTOs;

namespace PetFeast_Backend2.Services.UserService
{
    public interface IUserService
    {
        Task<List<UserResDTO>> GetUsers();
        Task<UserResDTO> GetUserById(int userId);
        Task<BlockUnblockRes> BlockOrUnblock(int userId);
    }
}
