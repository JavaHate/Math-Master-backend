using JavaHateBE.Data;
using JavaHateBE.Model;
using JavaHateBE.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JavaHateBE.Test.Repository
{
    public class GameRepositoryTest
    {
        private MathMasterDBContext CreateFakeDbContext()
        {
            var options = new DbContextOptionsBuilder<MathMasterDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new MathMasterDBContext(options);
        }

        private readonly GameMode ValidGameMode = GameMode.ENDLESS;
        private readonly string ValidUsername = "username";
        private readonly string ValidPassword = "password";
        private readonly string ValidEmail = "email@email.com";


        [Fact]
        public async Task GetAllGames_WithEmptyDatabase_ReturnsEmptyList()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            // Act
            var Games = await GameRepository.GetAllGames();
            // Assert
            Assert.Empty(Games);
        }

        [Fact]
        public async Task AddGame_WithValidGame_AddsGame()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            var ValidGame = new Game(ValidGameMode, ValidUser.Id);
            // Act
            await GameRepository.AddGame(ValidGame);
            var Game = await FakeDbContext.Games.FirstOrDefaultAsync(g => g.Id == ValidGame.Id);
            // Assert
            Assert.Equal(ValidGame, Game);
        }

        [Fact]
        public async Task GetGameById_WithValidId_ReturnsGame()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            var ValidGame = new Game(ValidGameMode, ValidUser.Id);
            await GameRepository.AddGame(ValidGame);
            // Act
            var Game = await GameRepository.GetGameById(ValidGame.Id);
            // Assert
            Assert.NotNull(Game);
        }

        [Fact]
        public async Task GetGameById_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            // Act
            var Game = await GameRepository.GetGameById(Guid.NewGuid());
            // Assert
            Assert.Null(Game);
        }

        [Fact]
        public async Task RemoveGame_WithValidId_RemovesGame()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            var ValidGame = new Game(ValidGameMode, ValidUser.Id);
            await GameRepository.AddGame(ValidGame);
            // Act
            await GameRepository.RemoveGame(ValidGame.Id);
            var Game = await GameRepository.GetGameById(ValidGame.Id);
            // Assert
            Assert.Null(Game);
        }

        [Fact]
        public async Task RemoveGame_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            // Act
            var Game = await GameRepository.RemoveGame(Guid.NewGuid());
            // Assert
            Assert.Null(Game);
        }
        [Fact]
        public async Task GetAllGames_WithGamesInDatabase_ReturnsGames()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            var ValidGame1 = new Game(ValidGameMode, ValidUser.Id);
            var ValidGame2 = new Game(ValidGameMode, ValidUser.Id);
            await GameRepository.AddGame(ValidGame1);
            await GameRepository.AddGame(ValidGame2);
            // Act
            var Games = await GameRepository.GetAllGames();
            // Assert
            Assert.Contains(ValidGame1, Games);
            Assert.Contains(ValidGame2, Games);
        }

        [Fact]
        public async Task GetGamesByUserId_WithValidUserId_ReturnsGames()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            var ValidGame = new Game(ValidGameMode, ValidUser.Id);
            await GameRepository.AddGame(ValidGame);
            // Act
            var Games = await GameRepository.GetGamesByUser(ValidUser.Id);
            // Assert
            Assert.Single(Games);
            Assert.Contains(ValidGame, Games);
        }

        [Fact]
        public async Task GetGamesByUserId_WithInvalidUserId_ReturnsEmptyList()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            // Act
            var Games = await GameRepository.GetGamesByUser(Guid.NewGuid());
            // Assert
            Assert.Empty(Games);
        }

        [Fact]
        public async Task GetGamesByGameMode_WithValidGameMode_ReturnsGames()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            var ValidGame1 = new Game(ValidGameMode, ValidUser.Id);
            var ValidGame2 = new Game(GameMode.FIVE_MINUTES, ValidUser.Id);
            await GameRepository.AddGame(ValidGame1);
            await GameRepository.AddGame(ValidGame2);
            // Act
            var Games = await GameRepository.GetGamesByGameMode(ValidGameMode);
            // Assert
            Assert.Single(Games);
            Assert.Contains(ValidGame1, Games);
        }

        [Fact]
        public async Task GetGamesByGameMode_WithInvalidGameMode_ReturnsEmptyList()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            // Act
            var Games = await GameRepository.GetGamesByGameMode(GameMode.FIVE_MINUTES);
            // Assert
            Assert.Empty(Games);
        }

        [Fact]
        public async Task GetGamesByGameModeAndUserId_WithValidGameModeAndUserId_ReturnsGames()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            var ValidGame1 = new Game(ValidGameMode, ValidUser.Id);
            var ValidGame2 = new Game(GameMode.FIVE_MINUTES, ValidUser.Id);
            await GameRepository.AddGame(ValidGame1);
            await GameRepository.AddGame(ValidGame2);
            // Act
            var Games = await GameRepository.GetGamesByUserByGameMode(ValidUser.Id, ValidGameMode);
            // Assert
            Assert.Single(Games);
            Assert.Contains(ValidGame1, Games);
        }

        [Fact]
        public async Task GetGamesByGameModeAndUserId_WithInvalidGameModeAndUserId_ReturnsEmptyList()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            // Act
            var Games = await GameRepository.GetGamesByUserByGameMode(Guid.NewGuid(), GameMode.FIVE_MINUTES);
            // Assert
            Assert.Empty(Games);
        }

        [Fact]
        public async Task UpdateGame_WithValidGame_UpdatesGame()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            var ValidGame = new Game(ValidGameMode, ValidUser.Id);
            await GameRepository.AddGame(ValidGame);
            var newGameMode = GameMode.FIVE_MINUTES;
            // Act
            ValidGame.updateGameMode(newGameMode);
            await GameRepository.UpdateGame(ValidGame);
            var Game = await GameRepository.GetGameById(ValidGame.Id);
            // Assert
            Assert.Equal(newGameMode, Game.GameMode);
        }

        [Fact]
        public async Task UpdateGame_WithInvalidGame_ReturnsNull()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var GameRepository = new GameRepository(FakeDbContext);
            var ValidUser = new User(ValidUsername, ValidPassword, ValidEmail);
            var InvalidGame = new Game(ValidGameMode, ValidUser.Id);
            // Act
            var Game = await GameRepository.UpdateGame(InvalidGame);
            // Assert
            Assert.Null(Game);
        }

    }
}