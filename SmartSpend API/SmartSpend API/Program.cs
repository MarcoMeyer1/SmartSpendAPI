using Microsoft.Data.SqlClient;
using SmartSpend_API.Services;

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
            builder.Services.AddScoped<GoalRepository>();

            // Add CORS configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Add Authorization (optional if you're using roles or other forms of authorization)
            builder.Services.AddAuthorization();

            // Add Swagger for API documentation
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Enable Swagger in Development environment
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartSpend_API v1");
                });
            }
            else
            {
                // Handle errors in production environment
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Enable HTTPS redirection
            app.UseHttpsRedirection();

            // Add CORS to the middleware pipeline
            app.UseCors("AllowAll");

            // Enable routing and map controllers
            app.UseRouting();

            // Enable Authorization middleware (if needed)
            app.UseAuthorization();

            // Map Controllers
            app.MapControllers();

            // Run the application
            app.Run();
        }
    }
}
