using System.Net;
using JavaHateBE.Controller;
using JavaHateBE.Data;
using JavaHateBE.Model;
using JavaHateBE.Model.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
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
                        options.UseInMemoryDatabase("TestDatabaseUser");
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

        [Fact]
        public async Task CreateUser_WithDuplicateUsername_ReturnsBadRequest()
        {
            // Arrange
            var OGuser = new UserCreateInput("test", "testoste", "valid@email.com");
            var createOriginal = await _client.PostAsJsonAsync("/user", OGuser);
            createOriginal.EnsureSuccessStatusCode();
            var user = new UserCreateInput("test", "testoste", "valleid@email.com");

            // Act
            var response = await _client.PostAsJsonAsync("/user", user);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateUser_WithDuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            var OGuser = new UserCreateInput("test", "testoste", "valid@email.com");
            var createOriginal = await _client.PostAsJsonAsync("/user", OGuser);
            createOriginal.EnsureSuccessStatusCode();
            var user = new UserCreateInput("test2", "testoste", "valid@email.com");

            // Act
            var response = await _client.PostAsJsonAsync("/user", user);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_WithValidId_ReturnsDeletedUser()
        {
            // Arrange
            var user = new UserCreateInput("test", "testoste", "valid@email.com");
            var createOriginal = await _client.PostAsJsonAsync("/user", user);
            createOriginal.EnsureSuccessStatusCode();
            var createdUser = await createOriginal.Content.ReadAsAsync<User>();

            // Act
            var response = await _client.DeleteAsync($"/user/id/{createdUser.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var deltedUser = await response.Content.ReadAsAsync<User>();
            Assert.NotNull(deltedUser);
            Assert.Equal(createdUser.Id, deltedUser.Id);
        }

        [Fact]
        public async Task DeleteUser_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var response = await _client.DeleteAsync($"/user/id/{userId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithValidUserNameAndPassword_ReturnsUser()
        {
            // Arrange
            var password = "testoste";
            var user = new UserCreateInput("test", password, "valid@email.com");
            var createOriginal = await _client.PostAsJsonAsync("/user", user);
            createOriginal.EnsureSuccessStatusCode();
            var createdUser = await createOriginal.Content.ReadAsAsync<User>();
            var login = new UserLogin(createdUser.Username, password);

            // Act
            var response = await _client.PostAsJsonAsync("/user/login", login);

            // Assert
            response.EnsureSuccessStatusCode();
            var loggedInUser = await response.Content.ReadAsAsync<User>();
            Assert.NotNull(loggedInUser);
            Assert.Equal(createdUser.Id, loggedInUser.Id);
        }

        [Fact]
        public async Task Login_WithValidEmailAndPassword_ReturnsUser()
        {
            // Arrange
            var password = "testoste";
            var user = new UserCreateInput("test", password, "valid@email.com");
            var createOriginal = await _client.PostAsJsonAsync("/user", user);
            createOriginal.EnsureSuccessStatusCode();
            var createdUser = await createOriginal.Content.ReadAsAsync<User>();
            var login = new UserLogin(createdUser.Email, password);

            // Act
            var response = await _client.PostAsJsonAsync("/user/login", login);

            // Assert
            response.EnsureSuccessStatusCode();
            var loggedInUser = await response.Content.ReadAsAsync<User>();
            Assert.NotNull(loggedInUser);
            Assert.Equal(createdUser.Id, loggedInUser.Id);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("test@testtest.com")]
        public async Task Login_WithInvalidUserNameOrEmail_ReturnsNotFound(string usernameOrEmail)
        {
            // Arrange
            var login = new UserLogin(usernameOrEmail, "password");

            // Act
            var response = await _client.PostAsJsonAsync("/user/login", login);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ReturnsBadRequest()
        {
            // Arrange
            var password = "testoste";
            var user = new UserCreateInput("test", password, "valid@email.com");
            var createOriginal = await _client.PostAsJsonAsync("/user", user);
            createOriginal.EnsureSuccessStatusCode();
            var createdUser = await createOriginal.Content.ReadAsAsync<User>();
            var login = new UserLogin(createdUser.Username, "password");

            // Act
            var response = await _client.PostAsJsonAsync("/user/login", login);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_WithValidUser_ReturnsUpdatedUser()
        {
            // Arrange
            var user = new UserCreateInput("test", "testoste", "valid@email.com");
            var createOriginal = await _client.PostAsJsonAsync("/user", user);
            createOriginal.EnsureSuccessStatusCode();
            var createdUser = await createOriginal.Content.ReadAsAsync<User>();
            var updatedUser = new User(createdUser.Id, "updated", "updated", "updated@updated.com", createdUser.LastLogin);

            // Act
            var response = await _client.PutAsJsonAsync("/user", updatedUser);

            // Assert
            response.EnsureSuccessStatusCode();
            var updatedUserResponse = await response.Content.ReadAsAsync<User>();
            Assert.NotNull(updatedUserResponse);
        }

        [Fact]
        public async Task UpdateUser_WithInvalidUser_ReturnsNotFound()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "test", "testoste", "test@test.com", DateTime.Now);

            // Act
            var response = await _client.PutAsJsonAsync("/user", user);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_WithDuplicateUsername_ReturnsBadRequest()
        {
            // Arrange
            var user = new UserCreateInput("test", "testoste", "test@test.com");
            var createOriginal = await _client.PostAsJsonAsync("/user", user);
            createOriginal.EnsureSuccessStatusCode();
            var createdUser = await createOriginal.Content.ReadAsAsync<User>();
            var updatedUser = new User(createdUser.Id, "David", "updated", "test2@test.com", DateTime.Now);

            // Act
            var response = await _client.PutAsJsonAsync("/user", updatedUser);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_WithDuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            var user = new UserCreateInput("test", "testoste", "test@test.com");
            var createOriginal = await _client.PostAsJsonAsync("/user", user);
            createOriginal.EnsureSuccessStatusCode();
            var createdUser = await createOriginal.Content.ReadAsAsync<User>();
            var updatedUser = new User(createdUser.Id, "test2", "updated", "david@example.com", DateTime.Now);

            // Act
            var response = await _client.PutAsJsonAsync("/user", updatedUser);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}