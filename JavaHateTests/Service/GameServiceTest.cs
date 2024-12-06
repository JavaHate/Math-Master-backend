using JavaHateBE.Data;
using JavaHateBE.Exceptions;
using JavaHateBE.Model;
using JavaHateBE.Model.DTOs;
using JavaHateBE.Repository;
using JavaHateBE.Service;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JavaHateBE.Test.Service
{
    public class GameServiceTest
    {
        private readonly string ValidText = "1 + 1";
        private readonly double ValidAnswer = 2;
        private readonly byte ValidDifficulty = 5;

        private readonly string ValidUsername = "TestUser";
        private readonly string ValidPassword = "TestPassword";
        private readonly string ValidEmail = "Test@email.com";

        private readonly GameMode ValidGameMode = GameMode.ENDLESS;

        private MathMasterDBContext CreateFakeDbContext()
        {
            var options = new DbContextOptionsBuilder<MathMasterDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new MathMasterDBContext(options);
        }

        [Fact]
        public async Task StartGame_WithValidUser_ReturnsGame()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);
            // Act
            var Game = await GameService.NewGame(ValidUser.Id, ValidGameMode.ToString());
            var User = await UserRepository.GetUserById(ValidUser.Id);
            // Assert
            Assert.NotNull(Game);
            Assert.Equal(ValidUser.Id, Game.UserId);
            Assert.Equal(ValidGameMode, Game.GameMode);
            Assert.Single(User.Games);
        }

        [Fact]
        public async Task StartGame_WithInvalidUser_ThrowsObjectNotFoundException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            // Act
            async Task<Game> StartGame() => await GameService.NewGame(Guid.NewGuid(), ValidGameMode.ToString());
            // Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(StartGame);
        }

        [Fact]
        public async Task StartGame_WithInvalidGameMode_ThrowsObjectNotFoundException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);
            // Act
            async Task<Game> StartGame() => await GameService.NewGame(ValidUser.Id, "INVALID");
            // Assert
            await Assert.ThrowsAsync<ArgumentException>(StartGame);
        }

        [Fact]
        public async Task GetGameById_WithValidId_ReturnsGame()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);
            var ValidGame = new Game(ValidGameMode, ValidUser.Id);
            await GameRepository.AddGame(ValidGame);
            // Act
            var Game = await GameService.GetGameById(ValidGame.Id);
            // Assert
            Assert.NotNull(Game);
            Assert.Equal(ValidGame, Game);
        }

        [Fact]
        public async Task GetGameById_WithInvalidId_ThrowsObjectNotFoundException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            // Act
            async Task<Game> GetGame() => await GameService.GetGameById(Guid.NewGuid());
            // Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(GetGame);
        }

        [Fact]
        public async Task AddGame_AddsGameToDb() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);
            var ValidGame = new Game(ValidGameMode, ValidUser.Id);
            // Act
            var Game = await GameService.AddGame(ValidGame);
            var User = await UserRepository.GetUserById(ValidUser.Id);
            // Assert
            Assert.NotNull(Game);
            Assert.Equal(ValidGame, Game);
            Assert.Single(User.Games);
        }

        [Fact]
        public async Task AddGame_WithInvalidUser_ThrowsObjectNotFoundException() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            var ValidGame = new Game(ValidGameMode, Guid.NewGuid());
            // Act
            async Task<Game> AddGame() => await GameService.AddGame(ValidGame);
            // Assert
            var ex = await Assert.ThrowsAsync<ObjectNotFoundException>(AddGame);
            Assert.Equal("User", ex.Object);
            Assert.Equal("No user found with that ID", ex.Message);
        }

        [Fact]
        public async Task RemoveGame_WithValidId_RemovesGameFromDb() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);
            var ValidGame = new Game(ValidGameMode, ValidUser.Id);
            await GameRepository.AddGame(ValidGame);
            // Act
            var Game = await GameService.RemoveGame(ValidGame.Id);
            var User = await UserRepository.GetUserById(ValidUser.Id);
            // Assert
            Assert.NotNull(Game);
            Assert.Equal(ValidGame, Game);
            Assert.Empty(User.Games);
        }

        [Fact]
        public async Task RemoveGame_WithInvalidId_ThrowsObjectNotFoundException() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            // Act
            async Task<Game> RemoveGame() => await GameService.RemoveGame(Guid.NewGuid());
            // Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(RemoveGame);
        }

        [Fact]
        public async Task GetAllGames_WithFilledDatabase_ReturnsAllGames() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);
            var ValidGame1 = await GameService.NewGame(ValidUser.Id, ValidGameMode.ToString());
            var ValidGame2 = await GameService.NewGame(ValidUser.Id, ValidGameMode.ToString());

            // Act
            var Games = await GameService.GetAllGames();

            // Assert
            Assert.NotNull(Games);
            Assert.Contains(ValidGame1, Games);
            Assert.Contains(ValidGame2, Games);
        }

        [Fact]
        public async Task GetAllGames_WithEmptyDatabase_ReturnsEmptyList() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);

            // Act
            var Games = await GameService.GetAllGames();

            // Assert
            Assert.NotNull(Games);
            Assert.Empty(Games);
        }

        [Fact]
        public async Task GetGamesByUser_WithValidUser_ReturnsGames() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);
            var ValidGame = await GameService.NewGame(ValidUser.Id, ValidGameMode.ToString());

            // Act
            var Games = await GameService.GetGamesByUser(ValidUser.Id);

            // Assert
            Assert.NotNull(Games);
            Assert.Single(Games);
            Assert.Contains(ValidGame, Games);
        }

        [Fact]
        public async Task GetGamesByUser_WithInvalidUser_ThrowsObjectNotFoundException() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);

            // Act
            async Task<List<Game>> GetGamesByUser() => await GameService.GetGamesByUser(Guid.NewGuid());

            // Assert
            var ex = await Assert.ThrowsAsync<ObjectNotFoundException>(GetGamesByUser);
            Assert.Equal("User", ex.Object);
            Assert.Equal("No user found with that ID", ex.Message);
        }

        [Fact]
        public async Task GetGamesByGameMode_WithValidGameMode_ReturnsGames()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);
            var ValidGame1 = await GameService.NewGame(ValidUser.Id, ValidGameMode.ToString());
            var ValidGame2 = await GameService.NewGame(ValidUser.Id, ValidGameMode.ToString());

            // Act
            var Games = await GameService.GetGamesByGameMode(ValidGameMode.ToString());

            // Assert
            Assert.NotNull(Games);
            Assert.Contains(ValidGame1, Games);
            Assert.Contains(ValidGame2, Games);
        }

        [Fact]
        public async Task GetGamesByGameMode_WithInvalidGameMode_ThrowsArgumentException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);

            // Act
            async Task<List<Game>> GetGames() => await GameService.GetGamesByGameMode("INVALID");

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(GetGames);
        }

        [Fact]
        public async Task GetGamesByGameMode_WithNoGames_ReturnsEmptyList()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);

            // Act
            var Games = await GameService.GetGamesByGameMode(ValidGameMode.ToString());

            // Assert
            Assert.NotNull(Games);
            Assert.Empty(Games);
        }

        [Fact]
        public async Task GetGamesByUserByGameMode_WithValidUserAndGameMode_ReturnsGames()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);
            var ValidGame = await GameService.NewGame(ValidUser.Id, ValidGameMode.ToString());

            // Act
            var Games = await GameService.GetGamesByUserByGameMode(ValidUser.Id, ValidGameMode.ToString());

            // Assert
            Assert.NotNull(Games);
            Assert.Single(Games);
            Assert.Contains(ValidGame, Games);
        }

        [Fact]
        public async Task GetGamesByUserByGameMode_WithInvalidUser_ThrowsObjectNotFoundException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);

            // Act
            async Task<List<Game>> GetGamesByUserByGameMode() => await GameService.GetGamesByUserByGameMode(Guid.NewGuid(), ValidGameMode.ToString());

            // Assert
            var ex = await Assert.ThrowsAsync<ObjectNotFoundException>(GetGamesByUserByGameMode);
            Assert.Equal("User", ex.Object);
            Assert.Equal("No user found with that ID", ex.Message);
        }

        [Fact]
        public async Task GetGamesByUserByGameMode_WithInvalidGameMode_ThrowsArgumentException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);

            // Act
            async Task<List<Game>> GetGamesByUserByGameMode() => await GameService.GetGamesByUserByGameMode(ValidUser.Id, "INVALID");

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(GetGamesByUserByGameMode);
        }

        [Fact]
        public async Task GetGamesByUserByGameMode_WithNoGames_ReturnsEmptyList()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);

            // Act
            var Games = await GameService.GetGamesByUserByGameMode(ValidUser.Id, ValidGameMode.ToString());

            // Assert
            Assert.NotNull(Games);
            Assert.Empty(Games);
        }

        [Fact]
        public async Task UpdateGame_WithValidGame_ReturnsUpdatedGame()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);
            var ValidGame = new Game(ValidGameMode, ValidUser.Id);
            await GameRepository.AddGame(ValidGame);
            ValidGame.Score = 100; // Update some property

            // Act
            var UpdatedGame = await GameService.UpdateGame(ValidGame);

            // Assert
            Assert.NotNull(UpdatedGame);
            Assert.Equal(ValidGame.Id, UpdatedGame.Id);
            Assert.Equal((uint)100, (uint)UpdatedGame.Score);
        }

        [Fact]
        public async Task UpdateGame_WithInvalidUser_ThrowsObjectNotFoundException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            var InvalidGame = new Game(ValidGameMode, Guid.NewGuid()); 

            // Act
            async Task<Game> UpdateGame() => await GameService.UpdateGame(InvalidGame);

            // Assert
            var ex = await Assert.ThrowsAsync<ObjectNotFoundException>(UpdateGame);
            Assert.Equal("User", ex.Object);
            Assert.Equal("No user found with that ID", ex.Message);
        }

        [Fact]
        public async Task UpdateGame_WithInvalidGame_ThrowsObjectNotFoundException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var UserRepository = new UserRepository(FakeDbContext);
            var GameService = new GameService(GameRepository, UserRepository);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            await UserRepository.CreateUser(ValidUser);
            var InvalidGame = new Game(ValidGameMode, ValidUser.Id);

            // Act
            async Task<Game> UpdateGame() => await GameService.UpdateGame(InvalidGame);

            // Assert
            var ex = await Assert.ThrowsAsync<ObjectNotFoundException>(UpdateGame);
            Assert.Equal("Game", ex.Object);
            Assert.Equal("No game found with that ID", ex.Message);
        }
    }
}