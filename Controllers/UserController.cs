using UsersAuth.Models;
using UsersAuth.Enums;
using UsersAuth.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UsersAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
       public async Task<IActionResult> Register([FromBody] UserRegister userRegistration)
        {
            var (errorCode, message) = await _userService.RegisterUser(userRegistration.Username, userRegistration.Email, userRegistration.Password);
            if (errorCode != ErrorCode.Success)
            {
                return BadRequest(message);
            }

            return Ok(message); 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            var token = await _userService.AuthenticateUserAsync(userLogin.Username, userLogin.Password);

            if (token != null)
            {
                return Ok(new { Token = token }); 
            }

            return Unauthorized();
        }
    }
}
