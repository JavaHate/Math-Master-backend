using Xunit;
using JavaHateBE.Model;

namespace JavaHateBE.Test.Domain
{
    public class GameTest {
        private readonly GameMode ValidGameMode = GameMode.ENDLESS;
        private readonly User ValidUser = new User("username", "password", "email@email.com");

        private readonly Question ValidQuestion1 = new Question("1 + 1", 2);
        private readonly Question ValidQuestion2 = new Question("2 + 2", 4);

        [Fact]
        public void CreatingValidGame_CreatesGame() {
            var Game = new Game(ValidGameMode, ValidUser.Id);

            Assert.NotNull(Game);
            Assert.NotEqual(Guid.Empty, Game.Id);
            Assert.Equal(ValidGameMode, Game.GameMode);
            Assert.Equal(ValidUser.Id, Game.UserId);
            Assert.Empty(Game.Questions);
            Assert.NotEqual(DateTime.MinValue, Game.StartTime);
            Assert.Equal(DateTime.MinValue, Game.EndTime);
        }

        [Fact]
        public void AddingQuestion_AddsQuestion() {
            var Game = new Game(ValidGameMode, ValidUser.Id);

            Game.AddQuestion(ValidQuestion1);

            Assert.Single(Game.Questions);
            Assert.Contains(ValidQuestion1, Game.Questions);
            Assert.Single(Game.Questions);
        }

        [Fact]
        public void IncreasingScore_IncreasesScore() {
            var Game = new Game(ValidGameMode, ValidUser.Id);

            Game.IncreaseScore();

            Assert.Equal((uint)1, Game.Score);
        }

        [Fact]
        public void UpdatingEndTime_UpdatesEndTime() {
            var Game = new Game(ValidGameMode, ValidUser.Id);
            var endTime = DateTime.Now;

            Game.updateEndTime(endTime);

            Assert.Equal(endTime, Game.EndTime);
        }

        [Fact]
        public void UpdatingScore_UpdatesScore() {
            var Game = new Game(ValidGameMode, ValidUser.Id);
            uint newScore = 5;

            Game.updateScore(newScore);

            Assert.Equal(newScore, Game.Score);
        }

        [Fact]
        public void UpdatingQuestions_UpdatesQuestions() {
            var Game = new Game(ValidGameMode, ValidUser.Id);
            var newQuestions = new List<Question> { ValidQuestion1, ValidQuestion2 };

            Game.updateQuestions(newQuestions);

            Assert.Equal(newQuestions, Game.Questions);
        }

        [Fact]
        public void UpdatingGameMode_UpdatesGameMode() {
            var Game = new Game(ValidGameMode, ValidUser.Id);
            var newGameMode = GameMode.FIVE_MINUTES;

            Game.updateGameMode(newGameMode);

            Assert.Equal(newGameMode, Game.GameMode);
        }

        [Fact]
        public void UpdatingStartTime_UpdatesStartTime() {
            var Game = new Game(ValidGameMode, ValidUser.Id);
            var newStartTime = DateTime.Now;

            Game.updateStartTime(newStartTime);

            Assert.Equal(newStartTime, Game.StartTime);
        }

        [Fact]
        public void UpdatingUserId_UpdatesUserId() {
            var Game = new Game(ValidGameMode, ValidUser.Id);
            var newUserId = Guid.NewGuid();

            Game.updateUserId(newUserId);

            Assert.Equal(newUserId, Game.UserId);
        }

        [Fact]
        public void Equals_ReturnsTrue_WhenComparingTwoGamesWithSameId() {
            var Game1 = new Game(ValidGameMode, ValidUser.Id);
            var Game2 = Game1;

            Assert.True(Game1.Equals(Game2));
        }

        [Fact]
        public void Equals_ReturnsFalse_WhenComparingTwoGamesWithDifferentId() {
            var Game1 = new Game(ValidGameMode, ValidUser.Id);
            var Game2 = new Game(ValidGameMode, ValidUser.Id);

            Assert.False(Game1.Equals(Game2));
        }
    }
}