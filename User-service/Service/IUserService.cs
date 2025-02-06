using User_service.Domain;
using User_service.Dto;

namespace User_service.Service
{
    public interface IUserService
    {
        public Task<ServiceResultDto> RegisterAsync(User user);
        public Task<ServiceResultDto> LoginAsync(LoginRequestDto loginRequestDto);
    }
}
