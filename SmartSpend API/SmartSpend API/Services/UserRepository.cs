using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SmartSpend_API.Services
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Create a new user
        public async Task<bool> CreateUser(string firstName, string lastName, string email, string passwordHash, string passwordSalt, string phoneNumber)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Users (FirstName, Surname, Email, PasswordHash, PasswordSalt, PhoneNumber) 
                                 VALUES (@FirstName, @Surname, @Email, @PasswordHash, @PasswordSalt, @PhoneNumber)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@Surname", lastName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                cmd.Parameters.AddWithValue("@PasswordSalt", passwordSalt);
                cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber ?? (object)DBNull.Value);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // User Login method
        public async Task<bool> Login(string email, string passwordHash)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND PasswordHash = @PasswordHash";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                conn.Open();
                int result = (int)await cmd.ExecuteScalarAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Fetch user password salt by email
        public async Task<string> GetUserSalt(string email)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT PasswordSalt FROM Users WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();
                string salt = (string)await cmd.ExecuteScalarAsync();
                conn.Close();
                return salt;
            }
        }

        // Fetch user by email to validate if the email exists
        public async Task<bool> CheckUserExists(string email)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();
                int result = (int)await cmd.ExecuteScalarAsync();
                conn.Close();
                return result > 0;
            }
        }
    }
}
