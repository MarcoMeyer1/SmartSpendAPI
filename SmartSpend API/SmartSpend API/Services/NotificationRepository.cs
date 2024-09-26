using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SmartSpend_API.Models;
using Microsoft.Extensions.Configuration;

namespace SmartSpend_API.Services
{
    public class NotificationRepository
    {
        private readonly string _connectionString;

        public NotificationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Create a new notification
        public async Task<bool> CreateNotification(Notification notification)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Notifications (UserID, NotificationText, NotificationDate) 
                                 VALUES (@UserID, @NotificationText, @NotificationDate)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", notification.UserID);
                cmd.Parameters.AddWithValue("@NotificationText", notification.NotificationText);
                cmd.Parameters.AddWithValue("@NotificationDate", notification.NotificationDate);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Get notifications for a specific user
        public async Task<List<Notification>> GetNotificationsByUserID(int userID)
        {
            List<Notification> notifications = new List<Notification>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Notifications WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userID);

                conn.Open();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        Notification notification = new Notification
                        {
                            NotificationID = (int)reader["NotificationID"],
                            UserID = (int)reader["UserID"],
                            NotificationText = reader["NotificationText"].ToString(),
                            NotificationDate = (DateTime)reader["NotificationDate"]
                        };
                        notifications.Add(notification);
                    }
                }
                conn.Close();
            }
            return notifications;
        }

        // Update notification
        public async Task<bool> UpdateNotification(Notification notification)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Notifications 
                                 SET NotificationText = @NotificationText, NotificationDate = @NotificationDate
                                 WHERE NotificationID = @NotificationID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NotificationID", notification.NotificationID);
                cmd.Parameters.AddWithValue("@NotificationText", notification.NotificationText);
                cmd.Parameters.AddWithValue("@NotificationDate", notification.NotificationDate);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Delete a notification
        public async Task<bool> DeleteNotification(int notificationID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Notifications WHERE NotificationID = @NotificationID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NotificationID", notificationID);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }
    }
}
