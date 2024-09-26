using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SmartSpend_API.Models;
using Microsoft.Extensions.Configuration;

namespace SmartSpend_API.Services
{
    public class DetailedViewRepository
    {
        private readonly string _connectionString;

        public DetailedViewRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Create a new detailed view
        public async Task<bool> CreateDetailedView(DetailedView detailedView)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO DetailedView (UserID, CategoryID, TotalExpense, TotalIncome, MonthYear) 
                                 VALUES (@UserID, @CategoryID, @TotalExpense, @TotalIncome, @MonthYear)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", detailedView.UserID);
                cmd.Parameters.AddWithValue("@CategoryID", detailedView.CategoryID);
                cmd.Parameters.AddWithValue("@TotalExpense", detailedView.TotalExpense);
                cmd.Parameters.AddWithValue("@TotalIncome", detailedView.TotalIncome);
                cmd.Parameters.AddWithValue("@MonthYear", detailedView.MonthYear);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Get detailed view for a specific user and category
        public async Task<List<DetailedView>> GetDetailedViewsByUserID(int userID)
        {
            List<DetailedView> detailedViews = new List<DetailedView>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM DetailedView WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userID);

                conn.Open();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        DetailedView detailedView = new DetailedView
                        {
                            DetailedViewID = (int)reader["DetailedViewID"],
                            UserID = (int)reader["UserID"],
                            CategoryID = (int)reader["CategoryID"],
                            TotalExpense = (decimal)reader["TotalExpense"],
                            TotalIncome = (decimal)reader["TotalIncome"],
                            MonthYear = reader["MonthYear"].ToString()
                        };
                        detailedViews.Add(detailedView);
                    }
                }
                conn.Close();
            }
            return detailedViews;
        }

        // Update a detailed view
        public async Task<bool> UpdateDetailedView(DetailedView detailedView)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE DetailedView 
                                 SET TotalExpense = @TotalExpense, TotalIncome = @TotalIncome, MonthYear = @MonthYear
                                 WHERE DetailedViewID = @DetailedViewID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DetailedViewID", detailedView.DetailedViewID);
                cmd.Parameters.AddWithValue("@TotalExpense", detailedView.TotalExpense);
                cmd.Parameters.AddWithValue("@TotalIncome", detailedView.TotalIncome);
                cmd.Parameters.AddWithValue("@MonthYear", detailedView.MonthYear);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Delete a detailed view
        public async Task<bool> DeleteDetailedView(int detailedViewID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM DetailedView WHERE DetailedViewID = @DetailedViewID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DetailedViewID", detailedViewID);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }
    }
}
