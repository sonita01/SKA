using Microsoft.AspNetCore.Mvc;
using UsersAuth.DTOs;
using UsersAuth.Services;

namespace UserAuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var existingUser =  await _userService.GetUsername(registerDTO.Username);
            if (existingUser != null)
            {
                return BadRequest("Username already taken.");
            }
            var user = await _userService.Register(registerDTO);
            if (user == null) return BadRequest("Registration failed.");
            return Ok(user);
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _userService.Login(loginDTO);
            if (user == null) return Unauthorized("Invalid credentials.");
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUser(id);
            if (user == null) return NotFound("User not found.");
            return Ok(user);
        }
    }
}
