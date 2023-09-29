using Travalers.Entities;

namespace Travalers.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByNICAsync(string nIC);

        Task CreateUserAsync(User user);

        Task<User> GetUserById(string id);

        Task<List<User>> GetAll();
    }
}
