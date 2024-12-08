using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JavaHateBE.Data;
using JavaHateBE.Model;
using JavaHateBE.Model.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace JavaHateBE.Test.Integration
{
    public class GameIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public GameIntegrationTest(WebApplicationFactory<Program> factory)
        {

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.UseContentRoot("../../../");
                builder.ConfigureServices(services =>
                {
                    var serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkInMemoryDatabase()
                        .BuildServiceProvider();

                    services.AddDbContext<MathMasterDBContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDatabaseGame");
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


        private readonly string ValidText = "1 + 1";
        private readonly double ValidAnswer = 2;
        private readonly byte ValidDifficulty = 5;

        private readonly string ValidUsername = "TestUser";
        private readonly string ValidPassword = "TestPassword";
        private readonly string ValidEmail = "Test@email.com";

        private readonly GameMode ValidGamemode = GameMode.ENDLESS;

        [Fact]
        public async Task GetAllGames_WithEmptyDatabase_ReturnsEmptyList()
        {
            // Arrange
            // Act
            var response = await _client.GetAsync("/game/all");
            // Assert
            response.EnsureSuccessStatusCode();
            var games = await response.Content.ReadAsAsync<List<Game>>();
            Assert.Empty(games);
        }

        [Fact]
        public async Task AddGame_WithValidGame_AddsGame()
        {
            // Arrange
            var User = new User(ValidUsername, ValidPassword, ValidEmail);
            var registerUser = await _client.PostAsJsonAsync("/user", User);
            registerUser.EnsureSuccessStatusCode();
            var registeredUser = await registerUser.Content.ReadAsAsync<User>();
            var NewGame = new Game(ValidGamemode, registeredUser.Id);

            // Act
            var response = await _client.PostAsJsonAsync("/game", NewGame);
            
            // Assert
            response.EnsureSuccessStatusCode();
            var game = await response.Content.ReadAsAsync<Game>();
            Assert.Equal(NewGame.GameMode, game.GameMode);
            Assert.Equal(NewGame.UserId, game.UserId);
        }

        [Fact]
        public async Task AddGame_WithInvalidUser_ReturnsNotFound()
        {
            // Arrange
            var NewGame = new Game(ValidGamemode, Guid.NewGuid());

            // Act
            var response = await _client.PostAsJsonAsync("/game", NewGame);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task AddGame_WithInvalidGameMode_ReturnsBadRequest()
        {
            // Arrange
            var User = new User(ValidUsername, ValidPassword, ValidEmail);
            var registerUser = await _client.PostAsJsonAsync("/user", User);
            registerUser.EnsureSuccessStatusCode();
            var registeredUser = await registerUser.Content.ReadAsAsync<User>();
            var request = new HttpRequestMessage(HttpMethod.Post, "/game")
            {
                Content = new StringContent("{\"GameMode\": \"INVALID\", \"UserId\": \"" + registeredUser.Id + "\"}", Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetAllGames_WithGames_ReturnsGames()
        {
            // Arrange
            var User = new User(ValidUsername, ValidPassword, ValidEmail);
            var registerUser = await _client.PostAsJsonAsync("/user", User);
            registerUser.EnsureSuccessStatusCode();
            var registeredUser = await registerUser.Content.ReadAsAsync<User>();
            var NewGame = new Game(ValidGamemode, registeredUser.Id);
            var addGame = await _client.PostAsJsonAsync("/game", NewGame);
            addGame.EnsureSuccessStatusCode();

            // Act
            var response = await _client.GetAsync("/game/all");

            // Assert
            response.EnsureSuccessStatusCode();
            var games = await response.Content.ReadAsAsync<List<Game>>();
            Assert.NotEmpty(games);
        }

        [Fact]
        public async Task GetGameById_WithValidGame_ReturnsGame()
        {
            // Arrange
            var User = new UserCreateInput(ValidUsername, ValidPassword, ValidEmail);
            var registerUser = await _client.PostAsJsonAsync("/user", User);
            registerUser.EnsureSuccessStatusCode();
            var registeredUser = await registerUser.Content.ReadAsAsync<User>();
            var NewGame = new Game(ValidGamemode, registeredUser.Id);
            var addGame = await _client.PostAsJsonAsync("/game", NewGame);
            addGame.EnsureSuccessStatusCode();
            var addedGame = await addGame.Content.ReadAsAsync<Game>();

            // Act
            var response = await _client.GetAsync("/game/id/" + addedGame.Id);

            // Assert
            response.EnsureSuccessStatusCode();
            var game = await response.Content.ReadAsAsync<Game>();
            Assert.Equal(addedGame.Id, game.Id);
        }

        [Fact]
        public async Task GetGameById_WithInvalidGame_ReturnsNotFound()
        {
            // Arrange
            // Act
            var response = await _client.GetAsync("/game/id/" + Guid.NewGuid());

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task RemoveGame_WithValidGame_RemovesGame()
        {
            // Arrange
            var User = new User(ValidUsername, ValidPassword, ValidEmail);
            var registerUser = await _client.PostAsJsonAsync("/user", User);
            registerUser.EnsureSuccessStatusCode();
            var registeredUser = await registerUser.Content.ReadAsAsync<User>();
            var NewGame = new Game(ValidGamemode, registeredUser.Id);
            var addGame = await _client.PostAsJsonAsync("/game", NewGame);
            addGame.EnsureSuccessStatusCode();
            var addedGame = await addGame.Content.ReadAsAsync<Game>();

            // Act
            var response = await _client.DeleteAsync("/game/id/" + addedGame.Id);
            var checkRemoved = await _client.GetAsync("/game/id/" + addedGame.Id);

            // Assert
            response.EnsureSuccessStatusCode();
            var game = await response.Content.ReadAsAsync<Game>();
            Assert.Equal(addedGame.Id, game.Id);
            Assert.Equal(HttpStatusCode.NotFound, checkRemoved.StatusCode);
        }

        [Fact]
        public async Task RemoveGame_WithInvalidGame_ReturnsNotFound()
        {
            // Arrange
            // Act
            var response = await _client.DeleteAsync("/game/id/" + Guid.NewGuid());

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetGamesByUser_WithValidUser_ReturnsGames()
        {
            // Arrange
            var User = new User(ValidUsername, ValidPassword, ValidEmail);
            var registerUser = await _client.PostAsJsonAsync("/user", User);
            registerUser.EnsureSuccessStatusCode();
            var registeredUser = await registerUser.Content.ReadAsAsync<User>();
            var NewGame = new Game(ValidGamemode, registeredUser.Id);
            var addGame = await _client.PostAsJsonAsync("/game", NewGame);
            addGame.EnsureSuccessStatusCode();

            // Act
            var response = await _client.GetAsync("/game/user/" + registeredUser.Id);

            // Assert
            response.EnsureSuccessStatusCode();
            var games = await response.Content.ReadAsAsync<List<Game>>();
            Assert.NotEmpty(games);
        }

        [Fact]
        public async Task GetGamesByUser_WithInvalidUser_ReturnsNotFound()
        {
            // Arrange
            // Act
            var response = await _client.GetAsync("/game/user/" + Guid.NewGuid());

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetGamesByUserByGameMode_WithValidUserAndGameMode_ReturnsGames()
        {
            // Arrange
            var User = new User(ValidUsername, ValidPassword, ValidEmail);
            var registerUser = await _client.PostAsJsonAsync("/user", User);
            registerUser.EnsureSuccessStatusCode();
            var registeredUser = await registerUser.Content.ReadAsAsync<User>();
            var NewGame = new Game(ValidGamemode, registeredUser.Id);
            var addGame = await _client.PostAsJsonAsync("/game", NewGame);
            addGame.EnsureSuccessStatusCode();

            // Act
            var response = await _client.GetAsync("/game/user/" + registeredUser.Id + "/gameMode/" + ValidGamemode);

            // Assert
            response.EnsureSuccessStatusCode();
            var games = await response.Content.ReadAsAsync<List<Game>>();
            Assert.NotEmpty(games);
        }

        [Fact]
        public async Task GetGamesByUserByGameMode_WithInvalidUser_ReturnsNotFound()
        {
            // Arrange
            // Act
            var response = await _client.GetAsync("/game/user/" + Guid.NewGuid() + "/gameMode/" + ValidGamemode);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetGamesByUserByGameMode_WithInvalidGameMode_ReturnsBadRequest()
        {
            // Arrange
            var User = new User(ValidUsername, ValidPassword, ValidEmail);
            var registerUser = await _client.PostAsJsonAsync("/user", User);
            registerUser.EnsureSuccessStatusCode();
            var registeredUser = await registerUser.Content.ReadAsAsync<User>();

            // Act
            var response = await _client.GetAsync("/game/user/" + registeredUser.Id + "/gameMode/INVALID");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetGamesByGameMode_WithValidGameMode_ReturnsGames()
        {
            // Arrange
            var User = new User(ValidUsername, ValidPassword, ValidEmail);
            var registerUser = await _client.PostAsJsonAsync("/user", User);
            registerUser.EnsureSuccessStatusCode();
            var registeredUser = await registerUser.Content.ReadAsAsync<User>();
            var NewGame = new Game(ValidGamemode, registeredUser.Id);
            var addGame = await _client.PostAsJsonAsync("/game", NewGame);
            addGame.EnsureSuccessStatusCode();

            // Act
            var response = await _client.GetAsync("/game/gameMode/" + ValidGamemode);

            // Assert
            response.EnsureSuccessStatusCode();
            var games = await response.Content.ReadAsAsync<List<Game>>();
            Assert.NotEmpty(games);
        }

        [Fact]
        public async Task GetGamesByGameMode_WithInvalidGameMode_ReturnsBadRequest()
        {
            // Arrange
            // Act
            var response = await _client.GetAsync("/game/gameMode/INVALID");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateGame_WithValidGame_ReturnsUpdatedGame()
        {
            // Arrange
            var User = new UserCreateInput(ValidUsername, ValidPassword, ValidEmail);
            var registerUser = await _client.PostAsJsonAsync("/user", User);
            registerUser.EnsureSuccessStatusCode();
            var registeredUser = await registerUser.Content.ReadAsAsync<User>();
            var addGame = await _client.PostAsync("/game/user/" + registeredUser.Id + "/gameMode/" + ValidGamemode, null);
            addGame.EnsureSuccessStatusCode();
            var addedGame = await addGame.Content.ReadAsAsync<Game>();
            addedGame.Score = 10;

            // Act
            var response = await _client.PutAsJsonAsync("/game", addedGame);

            // Assert
            response.EnsureSuccessStatusCode();
            var game = await response.Content.ReadAsAsync<Game>();
            Assert.Equal(addedGame.Score, game.Score);
        }

        [Fact]
        public async Task UpdateGame_WithInvalidGame_ReturnsNotFound()
        {
            // Arrange
            var NewGame = new Game(ValidGamemode, Guid.NewGuid());

            // Act
            var response = await _client.PutAsJsonAsync("/game", NewGame);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task NewGame_WithValidUserAndGameMode_ReturnsNewGame()
        {
            // Arrange
            var User = new User(ValidUsername, ValidPassword, ValidEmail);
            var registerUser = await _client.PostAsJsonAsync("/user", User);
            registerUser.EnsureSuccessStatusCode();
            var registeredUser = await registerUser.Content.ReadAsAsync<User>();

            // Act
            var response = await _client.PostAsync("/game/user/" + registeredUser.Id + "/gameMode/" + ValidGamemode, null);

            // Assert
            response.EnsureSuccessStatusCode();
            var game = await response.Content.ReadAsAsync<Game>();
            Assert.Equal(ValidGamemode, game.GameMode);
            Assert.Equal(registeredUser.Id, game.UserId);
        }

        [Fact]
        public async Task NewGame_WithInvalidUser_ReturnsNotFound()
        {
            // Arrange
            // Act
            var response = await _client.PostAsync("/game/user/" + Guid.NewGuid() + "/gameMode/" + ValidGamemode, null);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task NewGame_WithInvalidGameMode_ReturnsBadRequest()
        {
            // Arrange
            var User = new User(ValidUsername, ValidPassword, ValidEmail);
            var registerUser = await _client.PostAsJsonAsync("/user", User);
            registerUser.EnsureSuccessStatusCode();
            var registeredUser = await registerUser.Content.ReadAsAsync<User>();

            // Act
            var response = await _client.PostAsync("/game/user/" + registeredUser.Id + "/gameMode/INVALID", null);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}