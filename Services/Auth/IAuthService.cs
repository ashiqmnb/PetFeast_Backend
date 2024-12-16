using PetFeast_Backend2.Models.UserModels;
using PetFeast_Backend2.Models.UserModels.DTOs;

namespace PetFeast_Backend2.Services.Auth
{
    public interface IAuthService
    {
        Task<string> Register(UserRegisterDTO userReg);
        Task<UserLoginResDTO> Login(UserLoginDTO userLogin);
    }
}
