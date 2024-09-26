using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SmartSpend_API.Models;
using Microsoft.Extensions.Configuration;

namespace SmartSpend_API.Services
{
    public class IncomeRepository
    {
        private readonly string _connectionString;

        public IncomeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Create a new income entry
        public async Task<bool> CreateIncome(Income income)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Income (UserID, IncomeReference, Amount, IncomeDate) 
                                 VALUES (@UserID, @IncomeReference, @Amount, @IncomeDate)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", income.UserID);
                cmd.Parameters.AddWithValue("@IncomeReference", income.IncomeReference);
                cmd.Parameters.AddWithValue("@Amount", income.Amount);
                cmd.Parameters.AddWithValue("@IncomeDate", income.IncomeDate);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Get income entries for a specific user
        public async Task<List<Income>> GetIncomeByUserID(int userID)
        {
            List<Income> incomeList = new List<Income>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Income WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userID);

                conn.Open();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        Income income = new Income
                        {
                            IncomeID = (int)reader["IncomeID"],
                            UserID = (int)reader["UserID"],
                            IncomeReference = reader["IncomeReference"].ToString(),
                            Amount = (decimal)reader["Amount"],
                            IncomeDate = (DateTime)reader["IncomeDate"]
                        };
                        incomeList.Add(income);
                    }
                }
                conn.Close();
            }
            return incomeList;
        }

        // Update an income entry
        public async Task<bool> UpdateIncome(Income income)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Income 
                                 SET IncomeReference = @IncomeReference, Amount = @Amount, 
                                     IncomeDate = @IncomeDate
                                 WHERE IncomeID = @IncomeID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IncomeID", income.IncomeID);
                cmd.Parameters.AddWithValue("@IncomeReference", income.IncomeReference);
                cmd.Parameters.AddWithValue("@Amount", income.Amount);
                cmd.Parameters.AddWithValue("@IncomeDate", income.IncomeDate);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Delete an income entry
        public async Task<bool> DeleteIncome(int incomeID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Income WHERE IncomeID = @IncomeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IncomeID", incomeID);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }
    }
}
