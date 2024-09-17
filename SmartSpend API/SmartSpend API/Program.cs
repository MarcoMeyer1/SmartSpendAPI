using Microsoft.Data.SqlClient;
using SmartSpend_API.Data;

namespace SmartSpend_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Get the connection string from appsettings.json for Azure SQL Database
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string is missing in appsettings.json");
            }

            // Add services to the container.
            builder.Services.AddControllers();

            // Register SqlConnection using the connection string from appsettings.json (for ADO.NET)
            builder.Services.AddTransient<SqlConnection>(_ => new SqlConnection(connectionString));

            // Register the UserRepository for dependency injection
            builder.Services.AddTransient<UserRepository>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            builder.Services.AddSwaggerGen();  // Ensure this line is present

            var app = builder.Build();

            // Configure the HTTP request pipeline for development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();  // Ensure this line is present
                app.UseSwaggerUI();  // Ensure this line is present
            }
            else
            {
                // Use a custom error handler for production
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Enable HTTPS redirection
            app.UseHttpsRedirection();

            // Map Controllers
            app.MapControllers();

            // Run the application
            app.Run();
        }
    }
}
