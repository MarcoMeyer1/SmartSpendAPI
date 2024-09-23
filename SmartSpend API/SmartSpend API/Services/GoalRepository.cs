using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using SmartSpend_API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace SmartSpend_API.Services
{
    public class GoalRepository
    {
        private readonly string _connectionString;

        public GoalRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Create a new goal
        public async Task<bool> CreateGoal(Goal goal)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Goals (UserID, GoalName, TotalAmount, SavedAmount, CompletionDate) 
                                 VALUES (@UserID, @GoalName, @TotalAmount, @SavedAmount, @CompletionDate)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", goal.UserID);
                cmd.Parameters.AddWithValue("@GoalName", goal.GoalName);
                cmd.Parameters.AddWithValue("@TotalAmount", goal.TotalAmount);
                cmd.Parameters.AddWithValue("@SavedAmount", goal.SavedAmount);
                cmd.Parameters.AddWithValue("@CompletionDate", goal.CompletionDate ?? (object)DBNull.Value);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Get goals for a specific user
        public async Task<List<Goal>> GetGoalsByUserID(int userID)
        {
            List<Goal> goals = new List<Goal>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Goals WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userID);

                conn.Open();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        Goal goal = new Goal
                        {
                            GoalID = (int)reader["GoalID"],
                            UserID = (int)reader["UserID"],
                            GoalName = reader["GoalName"].ToString(),
                            TotalAmount = (decimal)reader["TotalAmount"],
                            SavedAmount = (decimal)reader["SavedAmount"],
                            CompletionDate = reader["CompletionDate"] as DateTime?
                        };
                        goals.Add(goal);
                    }
                }
                conn.Close();
            }
            return goals;
        }

        // Update goal progress
        public async Task<bool> UpdateGoal(Goal goal)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Goals 
                                 SET GoalName = @GoalName, TotalAmount = @TotalAmount, 
                                     SavedAmount = @SavedAmount, CompletionDate = @CompletionDate
                                 WHERE GoalID = @GoalID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@GoalID", goal.GoalID);
                cmd.Parameters.AddWithValue("@GoalName", goal.GoalName);
                cmd.Parameters.AddWithValue("@TotalAmount", goal.TotalAmount);
                cmd.Parameters.AddWithValue("@SavedAmount", goal.SavedAmount);
                cmd.Parameters.AddWithValue("@CompletionDate", goal.CompletionDate ?? (object)DBNull.Value);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Delete a goal
        public async Task<bool> DeleteGoal(int goalID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Goals WHERE GoalID = @GoalID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@GoalID", goalID);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }
    }
}
