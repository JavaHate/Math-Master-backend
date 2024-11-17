using JavaHateBE.Util;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using JavaHateBE.Data;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFile(builder.Configuration.GetSection("Logging:File"));

builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)); });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Defined in util/DependencyInjections.cs
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