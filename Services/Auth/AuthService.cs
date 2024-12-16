using AutoMapper;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetFeast_Backend2;
using PetFeast_Backend2.Migrations;
using PetFeast_Backend2.Models.UserModels;
using PetFeast_Backend2.Models.UserModels.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetFeast_Backend2.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AuthService(IConfiguration config, AppDbContext context, IMapper mapper)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<string> Register(UserRegisterDTO userReg)
        {
            try
            {
                if(userReg == null)
                {
                    throw new ArgumentNullException("User data cannot be null");
                }

                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userReg.Email);
                if (existingUser != null)
                {
                    throw new Exception("User with the same email is already exist");
                }

                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(userReg.Password, salt);
                var userEntity = _mapper.Map<User>(userReg);
                userEntity.Password = hashPassword;
                _context.Users.Add(userEntity);
                await _context.SaveChangesAsync();

                return "User registerd successfully";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





        public async Task<UserLoginResDTO> Login(UserLoginDTO userLogin)
        {
            try
            {
                var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Email == userLogin.Email);

                if (userEntity == null || !ValidatePassword(userLogin.Password, userEntity.Password))
                {
                    throw new InvalidOperationException("Invalid email or password");
                }

                if(userEntity.IsBlocked == true)
                {
                    throw new InvalidOperationException("You Are Restricted Or Blocked");
                }

                var token = GenerateJwtToken(userEntity);

                var loginRes = new UserLoginResDTO
                {
                    Name = userEntity.Name,
                    Email = userEntity.Email,
                    Role = userEntity.Role,
                    IsBlocked = userEntity.IsBlocked,
                    Token = token
                };

                return loginRes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }   


        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var token = new JwtSecurityToken(
                    claims: claims,
                    signingCredentials: credentials,
                    expires: DateTime.UtcNow.AddHours(2)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool ValidatePassword(string password, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }
    }
}
