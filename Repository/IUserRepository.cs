using UsersAuth.Models;
using System.Threading.Tasks;

namespace UsersAuth.Repositories
{
    public interface IUserRepository
    {
        Task<User> RegisterUser(string username, string email, string password);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserById(int id);
        Task<User> AuthenticateUser(string username, string password);
    }
}
