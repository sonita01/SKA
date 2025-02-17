using UsersAuth.DTOs;

namespace UsersAuth.Services
{
    public interface IUserService
    {
        Task<UserDTO> Register(RegisterDTO registerDTO);
        Task<UserDTO> Login(LoginDTO loginDTO);
        Task<UserDTO> GetUser(int userId);
         Task<UserDTO> GetUsername(string username);
        
    }
}
