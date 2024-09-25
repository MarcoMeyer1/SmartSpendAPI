using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SmartSpend_API.Services;
using SmartSpend_API.Models;
using System.Security.Cryptography;

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

        // Registration endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel request)
        {
            // Check if the user already exists
            if (await _userRepository.CheckUserExists(request.Email))
            {
                return BadRequest("User with this email already exists.");
            }

            // Hash the password with a salt
            var passwordHash = HashPassword(request.Password, out string salt);

            // Create the new user
            var success = await _userRepository.CreateUser(request.FirstName, request.LastName, request.Email, passwordHash, salt, request.PhoneNumber);

            if (!success)
            {
                return BadRequest("User registration failed.");
            }

            return Ok("User registered successfully.");
        }

        // Login endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var salt = await _userRepository.GetUserSalt(request.Email);
            if (string.IsNullOrEmpty(salt))
            {
                return Unauthorized("Invalid email or password.");
            }

            var passwordHash = HashPasswordWithSalt(request.Password, salt);

            var success = await _userRepository.Login(request.Email, passwordHash);

            if (!success)
            {
                return Unauthorized("Invalid credentials.");
            }

            // Get user details to return the userID
            var user = await _userRepository.GetUserByEmail(request.Email);

            // Return JSON with the userID and success message
            return Ok(new { userID = user.UserID, message = "Login successful" });
        }



        // Hashing password with salt generation
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
                byte[] hashBytes = new byte[36];
                Array.Copy(saltBytes, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                return Convert.ToBase64String(hashBytes);
            }
        }

        // Hashing password with an existing salt
        private string HashPasswordWithSalt(string password, string salt)
        {
            // Convert the base64 salt back to byte array
            byte[] saltBytes = Convert.FromBase64String(salt);

            // Use PBKDF2 to hash the password with the provided salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20);
                byte[] hashBytes = new byte[36];
                Array.Copy(saltBytes, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
