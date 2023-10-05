using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Travalers.DTOs.User;
using Travalers.Entities;
using Travalers.Repository;

namespace Travalers.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (userDto.Password != userDto.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            var existingUser = await _userRepository.GetUserByNICAsync(userDto.NIC);

            if (existingUser != null)
            {
                return BadRequest("NIC already exists.");
            }

            string passwordHash = HashPassword(userDto.Password);

            var newUser = new User
            {
                Id = userDto.NIC, 
                Username = userDto.Username,
                PasswordHash = passwordHash,
                UserType = (Enums.UserType)1,
                NIC = userDto.NIC,
                IsActive = true

            };

            await _userRepository.CreateUserAsync(newUser);

            return Ok("Registration successful.");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            var user = await _userRepository.GetUserByNICAsync(userDto.NIC);

            if (user == null)
            {
                return BadRequest("Invalid username or password.");
            }

            if (VerifyPassword(userDto.Password, user.PasswordHash))
            {
                var token = GenerateJwtToken(user.Id, user.NIC);
                return Ok(new { Token = token });
            }

            return BadRequest("Invalid username or password.");
        }

        [HttpGet("GetUserById{id}")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await _userRepository.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            var user = await _userRepository.GetAll();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpDelete("deactivateUser{id}")]
        public async Task<ActionResult> DeactivateUser(string id)
        {
            var user = await _userRepository.GetUserById(id);

            if (user == null)
            {
                return NotFound("User not Found");
            }

            else
            {
                if(user.IsActive == false)
                {
                    user.IsActive = true;
                }
                else
                {
                    user.IsActive = false;
                }
                

                await _userRepository.UpdateUserAsync(user);

                return Ok("Train Deleted Successfully");
            }
        }
        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            return HashPassword(password) == passwordHash;
        }

        private string GenerateJwtToken(string userId, string nic)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, nic.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, nic),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
