using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SmartSpend_API.Models;
using Microsoft.Extensions.Configuration;

namespace SmartSpend_API.Services
{
    public class CategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Create a new category
        public async Task<bool> CreateCategory(Category category)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Categories (CategoryName, ColorCode, UserID, MaxBudget) 
                         VALUES (@CategoryName, @ColorCode, @UserID, @MaxBudget)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                cmd.Parameters.AddWithValue("@ColorCode", category.ColorCode);
                cmd.Parameters.AddWithValue("@UserID", category.UserID);
                cmd.Parameters.AddWithValue("@MaxBudget", category.MaxBudget);  // Include maxBudget

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Get categories by userID
        public async Task<List<Category>> GetCategoriesByUserID(int userID)
        {
            List<Category> categories = new List<Category>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Categories WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userID);

                conn.Open();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        Category category = new Category
                        {
                            CategoryID = (int)reader["CategoryID"],
                            CategoryName = reader["CategoryName"].ToString(),
                            ColorCode = reader["ColorCode"].ToString(),
                            UserID = (int)reader["UserID"],
                            MaxBudget = (decimal)reader["MaxBudget"]  // Retrieve maxBudget
                        };
                        categories.Add(category);
                    }
                }
                conn.Close();
            }
            return categories;
        }

        // Get all categories
        public async Task<List<Category>> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Categories";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        Category category = new Category
                        {
                            CategoryID = (int)reader["CategoryID"],
                            CategoryName = reader["CategoryName"].ToString(),
                            ColorCode = reader["ColorCode"].ToString(),
                            MaxBudget = (decimal)reader["MaxBudget"]  // Retrieve maxBudget
                        };
                        categories.Add(category);
                    }
                }
                conn.Close();
            }
            return categories;
        }

        // Update a category
        public async Task<bool> UpdateCategory(Category category)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Categories 
                                 SET CategoryName = @CategoryName, ColorCode = @ColorCode, MaxBudget = @MaxBudget
                                 WHERE CategoryID = @CategoryID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryID", category.CategoryID);
                cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                cmd.Parameters.AddWithValue("@ColorCode", category.ColorCode);
                cmd.Parameters.AddWithValue("@MaxBudget", category.MaxBudget);  // Include maxBudget

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }

        // Delete a category
        public async Task<bool> DeleteCategory(int categoryID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Categories WHERE CategoryID = @CategoryID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);

                conn.Open();
                int result = await cmd.ExecuteNonQueryAsync();
                conn.Close();
                return result > 0;
            }
        }
    }
}
