using JavaHateBE.Util;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using JavaHateBE.Data;

var builder = WebApplication.CreateBuilder(args);

// Add FileLoggerProvider
builder.Logging.ClearProviders();
builder.Logging.AddProvider(new FileLoggerProvider("logs.txt"));

// Configure logging levels
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.Warning);

builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)); });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//defined in util/DependencyInjections.cs
builder.Services.AddCustomServices();

builder.Services.AddDbContext<MathMasterDBContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MathMasterDBContext>();
    context.Database.Migrate();
    DatabaseSeeder.SeedDatabase(context);
}

app.Run();