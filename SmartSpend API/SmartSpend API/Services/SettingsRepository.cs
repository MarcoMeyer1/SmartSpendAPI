using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SmartSpend_API.Models;
using Microsoft.Extensions.Configuration;

namespace SmartSpend_API.Services
{
    public class SettingsRepository
    {
        private readonly string _connectionString;

        public SettingsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Gets settings for a specific user
        public async Task<Settings> GetSettingsByUserID(int userID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Settings WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userID);

                conn.Open();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        return new Settings
                        {
                            SettingID = (int)reader["SettingID"],
                            UserID = (int)reader["UserID"],
                            AllowNotifications = (bool)reader["AllowNotifications"],
                            AllowSSO = (bool)reader["AllowSSO"],  
                            Language = reader["Language"].ToString()
                        };
                    }
                }
                conn.Close();
                return null; 
            }
        }

        // Updates settings for a specific user
        public async Task<bool> UpdateSettings(Settings settings)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Settings 
                                 SET AllowNotifications = @AllowNotifications, 
                                     AllowSSO = @AllowSSO, 
                                     Language = @Language
                                 WHERE UserID = @UserID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AllowNotifications", settings.AllowNotifications);
                cmd.Parameters.AddWithValue("@AllowSSO", settings.AllowSSO);  
                cmd.Parameters.AddWithValue("@Language", settings.Language);
                cmd.Parameters.AddWithValue("@UserID", settings.UserID);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Adds settings for a new user
        public async Task<bool> AddSettings(Settings settings)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Settings (UserID, AllowNotifications, AllowSSO, Language) 
                                 VALUES (@UserID, @AllowNotifications, @AllowSSO, @Language)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", settings.UserID);
                cmd.Parameters.AddWithValue("@AllowNotifications", settings.AllowNotifications);
                cmd.Parameters.AddWithValue("@AllowSSO", settings.AllowSSO);  
                cmd.Parameters.AddWithValue("@Language", settings.Language);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Deletes settings for a user
        public async Task<bool> DeleteSettings(int userID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Settings WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userID);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }
    }
}
