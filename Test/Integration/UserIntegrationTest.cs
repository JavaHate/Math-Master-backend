using System.Net;
using JavaHateBE.Data;
using JavaHateBE.Model;
using JavaHateBE.Model.DTOs;
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
            Assert.Equal(userId, user.Id);
        }

        [Fact]
        public async Task GetUserById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"/user/id/{userId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetUserByUsername_WithValidUsername_ReturnsUser()
        {
            // Arrange
            string username;
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<MathMasterDBContext>();

                var storedUser = await db.Users.FirstOrDefaultAsync();
                if(storedUser != null)
                    username = storedUser.Username;
                else
                    username = "test";
            }

            // Act
            var response = await _client.GetAsync($"/user/username/{username}");

            // Assert
            response.EnsureSuccessStatusCode();
            var user = await response.Content.ReadAsAsync<User>();
            Assert.NotNull(user);
            Assert.Equal(username, user.Username);
        }

        [Fact]
        public async Task GetUserByUsername_WithInvalidUsername_ReturnsNotFound()
        {
            // Arrange
            string username = "test";

            // Act
            var response = await _client.GetAsync($"/user/username/{username}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetUserByEmail_WithValidEmail_ReturnsUser()
        {
            // Arrange
            string email;
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<MathMasterDBContext>();

                var storedUser = await db.Users.FirstOrDefaultAsync();
                if(storedUser != null)
                    email = storedUser.Email;
                else
                    email = "email";
            }

            // Act
            var response = await _client.GetAsync($"/user/email/{email}");

            // Assert
            response.EnsureSuccessStatusCode();
            var user = await response.Content.ReadAsAsync<User>();
            Assert.NotNull(user);
            Assert.Equal(email, user.Email);
        }

        [Fact]
        public async Task GetUserByEmail_WithInvalidEmail_ReturnsNotFound()
        {
            // Arrange
            string email = "email";

            // Act
            var response = await _client.GetAsync($"/user/email/{email}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateUser_WithValidUser_ReturnsUser()
        {
            // Arrange
            var user = new UserCreateInput("test", "testoste", "test@test.com");


            // Act
            var response = await _client.PostAsJsonAsync("/user", user);

            // Assert
            response.EnsureSuccessStatusCode();
            var createdUser = await response.Content.ReadAsAsync<User>();
            Assert.NotNull(createdUser);
            Assert.Equal(user.Username, createdUser.Username);
            Assert.Equal(user.Email, createdUser.Email);
        }
    }
}