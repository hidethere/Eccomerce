using User_service.Domain;

namespace User_service.Repository
{
    public interface IUserRepository
    {
        public Task<User> FindUserByIdAsync(Guid userId); 
        public Task<User> FindUserByEmailAsync(string email); 
        public Task<User> FindUserByUsernameAsync(string username); 
        public Task<User> AddAsync(User user);
    }
}
