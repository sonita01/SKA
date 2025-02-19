using UsersAuth.Models;
using System.Threading.Tasks;
using UsersAuth.Enums;

namespace UsersAuth.Services
{
    public interface IUserService
    {
        Task<(ErrorCode,String)> RegisterUser(string username, string email, string hashedPassword);
        Task<string> AuthenticateUserAsync(string username, string password);
        bool ValidatePassword(string password);
        string HashPassword(string password);
        string GenerateJwtToken(User user);
    }
}
