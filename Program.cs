using System.Text.Json;
using System.Text.Json.Serialization;
using JavaHateBE.Data;
using JavaHateBE.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JavaHateBE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add FileLoggerProvider
            builder.Logging.ClearProviders();
            builder.Logging.AddProvider(new FileLoggerProvider("logs.txt"));

            // Configure logging levels
            builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
            builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.Warning);

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCustomServices();

            var environment = builder.Environment;

            if (environment.IsEnvironment("Testing"))
            {
                builder.Services.AddDbContext<MathMasterDBContext>(options =>
                    options.UseInMemoryDatabase("TestDatabase"));
            }
            else
            {
                builder.Services.AddDbContext<MathMasterDBContext>(options =>
                    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
            }

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MathMasterDBContext>();
                if (dbContext.Database.IsRelational())
                {
                    dbContext.Database.Migrate();
                    DatabaseSeeder.SeedDatabase(dbContext);
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}