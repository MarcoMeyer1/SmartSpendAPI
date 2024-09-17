using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SmartSpend_API.Data;
using SmartSpend_API.DTOs;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SmartSpend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            var passwordHash = HashPassword(request.Password, out string salt);

            var success = await _userRepository.CreateUser(request.FirstName, request.LastName, request.Email, passwordHash, salt);

            if (!success)
            {
                return BadRequest("User registration failed.");
            }

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var salt = await _userRepository.GetUserSalt(request.Email);
            var passwordHash = HashPasswordWithSalt(request.Password, salt);

            var success = await _userRepository.Login(request.Email, passwordHash);

            if (!success)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok("Login successful.");
        }


        private string HashPassword(string password, out string salt)
        {
            // Generate a random salt
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }

            // Convert salt to a base64 string for storing
            salt = Convert.ToBase64String(saltBytes);

            // Use PBKDF2 to hash the password along with the salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20); // Generate a 20-byte hash
                                                   // Combine the salt and hash into one string for storage
                byte[] hashBytes = new byte[36];
                Array.Copy(saltBytes, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                // Return the combined salt+hash as a base64 string
                return Convert.ToBase64String(hashBytes);
            }
        }

        private string HashPasswordWithSalt(string password, string salt)
        {
            // Convert the base64 salt back to byte array
            byte[] saltBytes = Convert.FromBase64String(salt);

            // Use PBKDF2 to hash the password with the provided salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20); // Generate a 20-byte hash
                byte[] hashBytes = new byte[36];
                Array.Copy(saltBytes, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                // Return the combined salt+hash as a base64 string
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
