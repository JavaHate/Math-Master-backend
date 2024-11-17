using JavaHateBE.Data;
using JavaHateBE.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JavaHateBE.Test.Integration
{
    public class UserIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public UserIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.UseContentRoot("../../../../JavaHateBE");
                builder.ConfigureServices(services =>
                {
                    var serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkInMemoryDatabase()
                        .BuildServiceProvider();

                    services.AddDbContext<MathMasterDBContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDatabase");
                        options.UseInternalServiceProvider(serviceProvider);
                    });

                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<MathMasterDBContext>();

                        db.Database.EnsureCreated();

                        try
                        {
                            DatabaseSeeder.SeedDatabase(db);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            throw;
                        }
                    }
                });
            });
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetUserById_WithValidId_ReturnsUser()
        {
            // Arrange
            Guid userId;
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<MathMasterDBContext>();

                var storedUser = await db.Users.FirstOrDefaultAsync();
                if(storedUser != null)
                    userId = storedUser.Id;
                else
                    userId = Guid.NewGuid();
            }

            // Act
            var response = await _client.GetAsync($"/user/id/{userId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var user = await response.Content.ReadAsAsync<User>();
            Assert.NotNull(user);
        }

        
    }
}