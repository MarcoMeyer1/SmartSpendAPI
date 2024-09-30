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
            if (await _userRepository.CheckUserExists(request.Email))
            {
                return BadRequest("User with this email already exists.");
            }

            var passwordHash = HashPassword(request.Password, out string salt);

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

            // Gets user details to return the userID
            var user = await _userRepository.GetUserByEmail(request.Email);

            // Returns JSON with the userID and success message
            return Ok(new { userID = user.UserID, message = "Login successful" });
        }



        // Hashing a password with salt generation
        private string HashPassword(string password, out string salt)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }

            salt = Convert.ToBase64String(saltBytes);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20); 
                byte[] hashBytes = new byte[36];
                Array.Copy(saltBytes, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                return Convert.ToBase64String(hashBytes);
            }
        }

        // Hashing a password with an existing salt
        private string HashPasswordWithSalt(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20);
                byte[] hashBytes = new byte[36];
                Array.Copy(saltBytes, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                return Convert.ToBase64String(hashBytes);
            }
        }

        [HttpGet("{userID}")]
        public async Task<IActionResult> GetUserProfile(int userID)
        {
            var user = await _userRepository.GetUserByID(userID);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        // Updates user profile
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] User user)
        {
            bool success = await _userRepository.UpdateUser(user);
            if (!success)
            {
                return BadRequest("User update failed.");
            }
            return Ok("User profile updated successfully.");
        }

        // Deletes user account
        [HttpDelete("delete/{userID}")]
        public async Task<IActionResult> DeleteUser(int userID)
        {
            bool success = await _userRepository.DeleteUser(userID);
            if (!success)
            {
                return BadRequest("User deletion failed.");
            }
            return Ok("User account deleted successfully.");
        }
    }
}

