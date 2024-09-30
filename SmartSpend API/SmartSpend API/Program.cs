using Microsoft.Data.SqlClient;
using SmartSpend_API.Services;

namespace SmartSpend_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string is missing in appsettings.json");
            }

            builder.Services.AddControllers();

            builder.Services.AddTransient<SqlConnection>(_ => new SqlConnection(connectionString));

            builder.Services.AddTransient<UserRepository>();
            builder.Services.AddScoped<GoalRepository>();
            builder.Services.AddScoped<CategoryRepository>();
            builder.Services.AddScoped<DetailedViewRepository>();
            builder.Services.AddScoped<ExpenseRepository>();
            builder.Services.AddScoped<IncomeRepository>();
            builder.Services.AddScoped<NotificationRepository>();
            builder.Services.AddScoped<ReminderRepository>();
            builder.Services.AddScoped<SettingsRepository>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddAuthorization();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartSpend_API v1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
