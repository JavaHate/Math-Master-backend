using System.Text.Json;
using System.Text.Json.Serialization;
using JavaHateBE.Data;
using JavaHateBE.Util;
using Microsoft.EntityFrameworkCore;
namespace JavaHateBE
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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