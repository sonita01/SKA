using UsersAuth.Models;
using UsersAuth.Enums;
using UsersAuth.Repositories;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;

namespace UsersAuth.Services
{
    public class UserService : IUserService
    {
        private readonly string _secretKey = "aohteoahpeo14561235555555iooeiii"; 
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
        public bool ValidatePassword(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }

            var hasLetter = new System.Text.RegularExpressions.Regex(@"[a-zA-Z]").IsMatch(password);
            var hasDigit = new System.Text.RegularExpressions.Regex(@"[0-9]").IsMatch(password);

            return hasLetter && hasDigit;
        }

        public async Task<(ErrorCode, string)> RegisterUser(string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return (ErrorCode.UsernameOrPasswordEmpty, "Username and password cannot be empty");
            }
            if (!ValidatePassword(password))
            {
                return (ErrorCode.InvalidPassword, "Password must be at least 8 characters long and contain both letters and numbers.");
            }
            password = HashPassword(password);
            var result = await _userRepository.RegisterUser(username, email, password);
            if (result == null)
            {
                return (ErrorCode.RegistrationFailed, "User registration failed");
            }
            return (ErrorCode.Success, "User registered successfully");
        }

        public async Task<string> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.AuthenticateUser(username,password);

            if (user == null)
            {
                return null; 
            }

            return GenerateJwtToken(user); 
        }
        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                // issuer: "https://yourapp.com",
                // audience: "https://api.yourapp.com", 
                claims: claims,
                expires: DateTime.Now.AddHours(1), 
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token); 
        }

        public ClaimsPrincipal ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                // ValidIssuer = "https://yourapp.com",
                // ValidAudience = "https://api.yourapp.com",
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true, 
                ClockSkew = TimeSpan.Zero 
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return principal;
            }
            catch
            {
                return null; 
            }
        }
    }
}
