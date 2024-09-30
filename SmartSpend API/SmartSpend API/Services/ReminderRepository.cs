using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using SmartSpend_API.Models;
using Microsoft.Extensions.Configuration;

namespace SmartSpend_API.Services
{
    public class ReminderRepository
    {
        private readonly string _connectionString;

        public ReminderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Creates a new reminder
        public async Task<bool> CreateReminder(Reminder reminder)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Reminders (UserID, Description, DateDue, NotificationDate, IsEnabled, IsCompleted) 
                                 VALUES (@UserID, @Description, @DateDue, @NotificationDate, @IsEnabled, @IsCompleted)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", reminder.UserID);
                cmd.Parameters.AddWithValue("@Description", reminder.Description);
                cmd.Parameters.AddWithValue("@DateDue", reminder.DateDue);
                cmd.Parameters.AddWithValue("@NotificationDate", reminder.NotificationDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IsEnabled", reminder.IsEnabled);
                cmd.Parameters.AddWithValue("@IsCompleted", reminder.IsCompleted);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Gets reminders for a specific user
        public async Task<List<Reminder>> GetRemindersByUserID(int userID)
        {
            List<Reminder> reminders = new List<Reminder>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Reminders WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userID);

                conn.Open();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        Reminder reminder = new Reminder
                        {
                            ReminderID = (int)reader["ReminderID"],
                            UserID = (int)reader["UserID"],
                            Description = reader["Description"].ToString(),
                            DateDue = (DateTime)reader["DateDue"],
                            NotificationDate = reader["NotificationDate"] as DateTime?,
                            IsEnabled = (bool)reader["IsEnabled"],
                            IsCompleted = (bool)reader["IsCompleted"] // Retrieve IsCompleted value
                        };
                        reminders.Add(reminder);
                    }
                }
                conn.Close();
            }
            return reminders;
        }

        // Updates reminder
        public async Task<bool> UpdateReminder(Reminder reminder)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Reminders 
                                 SET Description = @Description, DateDue = @DateDue, 
                                     NotificationDate = @NotificationDate, IsEnabled = @IsEnabled, IsCompleted = @IsCompleted
                                 WHERE ReminderID = @ReminderID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ReminderID", reminder.ReminderID);
                cmd.Parameters.AddWithValue("@Description", reminder.Description);
                cmd.Parameters.AddWithValue("@DateDue", reminder.DateDue);
                cmd.Parameters.AddWithValue("@NotificationDate", reminder.NotificationDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IsEnabled", reminder.IsEnabled);
                cmd.Parameters.AddWithValue("@IsCompleted", reminder.IsCompleted);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Deletes a reminder
        public async Task<bool> DeleteReminder(int reminderID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Reminders WHERE ReminderID = @ReminderID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ReminderID", reminderID);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }
    }
}
