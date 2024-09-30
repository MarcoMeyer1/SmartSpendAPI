using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SmartSpend_API.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace SmartSpend_API.Services
{
    public class ExpenseRepository
    {
        private readonly string _connectionString;

        public ExpenseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Creates a new expense
        public async Task<bool> CreateExpense(Expense expense)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Expenses (UserID, ExpenseName, CategoryID, Amount, ExpenseDate) 
                                 VALUES (@UserID, @ExpenseName, @CategoryID, @Amount, @ExpenseDate)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", expense.UserID);
                cmd.Parameters.AddWithValue("@ExpenseName", expense.ExpenseName);
                cmd.Parameters.AddWithValue("@CategoryID", expense.CategoryID);
                cmd.Parameters.AddWithValue("@Amount", expense.Amount);
                cmd.Parameters.AddWithValue("@ExpenseDate", expense.ExpenseDate);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Gets expenses for a specific user
        public async Task<List<Expense>> GetExpensesByUserID(int userID)
        {
            List<Expense> expenses = new List<Expense>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Expenses WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userID);

                conn.Open();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        Expense expense = new Expense
                        {
                            ExpenseID = (int)reader["ExpenseID"],
                            UserID = (int)reader["UserID"],
                            ExpenseName = reader["ExpenseName"].ToString(),
                            CategoryID = (int)reader["CategoryID"],
                            Amount = (decimal)reader["Amount"],
                            ExpenseDate = (DateTime)reader["ExpenseDate"]
                        };
                        expenses.Add(expense);
                    }
                }
                conn.Close();
            }
            return expenses;
        }

        // Gets expenses by category ID and user ID
        public async Task<List<Expense>> GetExpensesByCategoryIDAndUserID(int categoryID, int userID)
        {
            List<Expense> expenses = new List<Expense>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Expenses WHERE CategoryID = @CategoryID AND UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                cmd.Parameters.AddWithValue("@UserID", userID);

                conn.Open();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        Expense expense = new Expense
                        {
                            ExpenseID = (int)reader["ExpenseID"],
                            UserID = (int)reader["UserID"],
                            ExpenseName = reader["ExpenseName"].ToString(),
                            CategoryID = (int)reader["CategoryID"],
                            Amount = (decimal)reader["Amount"],
                            ExpenseDate = (DateTime)reader["ExpenseDate"]
                        };
                        expenses.Add(expense);
                    }
                }
                conn.Close();
            }
            return expenses;
        }

        // Updates an expense
        public async Task<bool> UpdateExpense(Expense expense)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Expenses 
                                 SET ExpenseName = @ExpenseName, CategoryID = @CategoryID, 
                                     Amount = @Amount, ExpenseDate = @ExpenseDate
                                 WHERE ExpenseID = @ExpenseID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ExpenseID", expense.ExpenseID);
                cmd.Parameters.AddWithValue("@ExpenseName", expense.ExpenseName);
                cmd.Parameters.AddWithValue("@CategoryID", expense.CategoryID);
                cmd.Parameters.AddWithValue("@Amount", expense.Amount);
                cmd.Parameters.AddWithValue("@ExpenseDate", expense.ExpenseDate);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Deletes an expense
        public async Task<bool> DeleteExpense(int expenseID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Expenses WHERE ExpenseID = @ExpenseID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ExpenseID", expenseID);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Gets total expenses per category for a user
        public async Task<List<CategoryExpenseTotal>> GetTotalExpensesPerCategoryForUser(int userID)
        {
            List<CategoryExpenseTotal> categoryTotals = new List<CategoryExpenseTotal>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT c.CategoryID, c.CategoryName, c.ColorCode, c.MaxBudget, 
                           ISNULL(SUM(e.Amount), 0) AS TotalSpent
                    FROM Categories c
                    LEFT JOIN Expenses e ON c.CategoryID = e.CategoryID AND e.UserID = @UserID
                    WHERE c.UserID = @UserID
                    GROUP BY c.CategoryID, c.CategoryName, c.ColorCode, c.MaxBudget";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userID);

                conn.Open();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        CategoryExpenseTotal total = new CategoryExpenseTotal
                        {
                            CategoryID = (int)reader["CategoryID"],
                            CategoryName = reader["CategoryName"].ToString(),
                            ColorCode = reader["ColorCode"].ToString(),
                            MaxBudget = (decimal)reader["MaxBudget"],
                            TotalSpent = (decimal)reader["TotalSpent"]
                        };
                        categoryTotals.Add(total);
                    }
                }
                conn.Close();
            }
            return categoryTotals;
        }
    }
}
