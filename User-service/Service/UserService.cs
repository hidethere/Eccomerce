using Microsoft.AspNetCore.Identity;
using User_service.Domain;
using User_service.Dto;
using User_service.Helper;
using User_service.Persistence;
using User_service.Repository;

namespace User_service.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly GenerateJwtHelper _generateJwtHelper;
        public UserService(IUserRepository userRepository, GenerateJwtHelper generateJwtHelper) 
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
            _generateJwtHelper = generateJwtHelper;
        }

        public async Task<ServiceResultDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            User userFound = await _userRepository.FindUserByEmailAsync(loginRequestDto.Email);

            if(userFound == null)
            {
                return new ServiceResultDto()
                {
                    Success = false,
                    Message = "Email Doesnt Exist!"
                };
            }

            var result = _passwordHasher.VerifyHashedPassword(null, userFound.PasswordHash, loginRequestDto.Password);
            if(result != PasswordVerificationResult.Success)
            {
                return new ServiceResultDto()
                {
                    Success = false,
                    Message = "Email/Password combination doesnt match!",
                };
            }

            var token = _generateJwtHelper.GenerateJwt(userFound);

            return new ServiceResultDto()
            {
                Success = true,
                Message = "Login successfull!",
                Data = token

            };

        }

        public async Task<ServiceResultDto> RegisterAsync(User user)
        {
            User emailFound = await _userRepository.FindUserByEmailAsync(user.Email);
            User usernameFound = await _userRepository.FindUserByUsernameAsync(user.Username);

            if (emailFound != null || usernameFound != null) 
            {
                return new ServiceResultDto
                {
                    Success = false,
                    Message = "Email/Username already exists!"
                };
            }


            user.PasswordHash = _passwordHasher.HashPassword(null, user.PasswordHash);
            await _userRepository.AddAsync(user);

            return new ServiceResultDto
            {
                Success = true,
                Message = "User created!!",
                Data = user
            };

        }
    }
}
