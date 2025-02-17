using UsersAuth.DTOs;
using UsersAuth.Models;
using UsersAuth.Repositories;
using System.Threading.Tasks;

namespace UsersAuth.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO> Register(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
                throw new ArgumentNullException(nameof(registerDTO));

            var user = await _userRepository.RegisterUser(registerDTO.Username, registerDTO.Email, registerDTO.Password);

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username
            };
        }

        public async Task<UserDTO> Login(LoginDTO loginDTO)
        {
            if (loginDTO == null)
                throw new ArgumentNullException(nameof(loginDTO));

            var user = await _userRepository.AuthenticateUser(loginDTO.Username, loginDTO.Password);

            if (user == null)
                return null;

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username
            };
        }

        public async Task<UserDTO> GetUser(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return null;

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username
            };
        }
         public async Task<UserDTO> GetUsername(string username)
        {
            var user = await _userRepository.GetUserByUsername(username);
            if (user == null) return null;

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username
            };
        }
    }
}
