using JavaHateBE.Data;
using JavaHateBE.Model;
using JavaHateBE.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JavaHateBE.Test.Repository
{
    public class QuestionRepositoryTest
    {
        private MathMasterDBContext CreateFakeDbContext()
        {
            var options = new DbContextOptionsBuilder<MathMasterDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new MathMasterDBContext(options);
        }

        private readonly string ValidText = "1 + 1";
        private readonly double ValidAnswer = 2;
        private readonly byte ValidDifficulty = 5;

        [Fact]
        public async Task GetAllGames_WithEmptyDatabase_ReturnsEmptyList()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            // Act
            var Questions = await QuestionRepository.GetAllQuestions();
            // Assert
            Assert.Empty(Questions);
        }

        [Fact]
        public async Task AddQuestion_WithValidArguments_AddsQuestion()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var ValidQuestion = new Question(ValidText, ValidAnswer, ValidDifficulty);
            // Act
            await QuestionRepository.AddQuestion(ValidQuestion);
            var Questions = await QuestionRepository.GetAllQuestions();
            // Assert
            Assert.Single(Questions);
        }

        [Fact]
        public async Task GetQuestionById_WithValidId_ReturnsQuestion()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var ValidQuestion = new Question(ValidText, ValidAnswer, ValidDifficulty);
            await QuestionRepository.AddQuestion(ValidQuestion);
            // Act
            var Question = await QuestionRepository.GetQuestionById(ValidQuestion.Id);
            // Assert
            Assert.Equal(ValidQuestion, Question);
        }

        [Fact]
        public async Task GetQuestionById_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            // Act
            var Question = await QuestionRepository.GetQuestionById(Guid.NewGuid());
            // Assert
            Assert.Null(Question);
        }

        [Fact]
        public async Task UpdateQuestion_WithValidQuestion_UpdatesQuestion()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var ValidQuestion = new Question(ValidText, ValidAnswer, ValidDifficulty);
            await QuestionRepository.AddQuestion(ValidQuestion);
            ValidQuestion.UpdateText("2 + 2");
            // Act
            await QuestionRepository.UpdateQuestion(ValidQuestion);
            var Question = await QuestionRepository.GetQuestionById(ValidQuestion.Id);
            // Assert
            Assert.Equal(ValidQuestion, Question);
        }

        [Fact]
        public async Task UpdateQuestion_WithInvalidQuestion_ReturnsNull()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var ValidQuestion = new Question(ValidText, ValidAnswer, ValidDifficulty);
            // Act
            var Question = await QuestionRepository.UpdateQuestion(ValidQuestion);
            // Assert
            Assert.Null(Question);
        }

        [Fact]
        public async Task DeleteQuestion_WithValidId_DeletesQuestion()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var ValidQuestion = new Question(ValidText, ValidAnswer, ValidDifficulty);
            await QuestionRepository.AddQuestion(ValidQuestion);
            // Act
            await QuestionRepository.DeleteQuestion(ValidQuestion.Id);
            var Question = await QuestionRepository.GetQuestionById(ValidQuestion.Id);
            // Assert
            Assert.Null(Question);
        }

        [Fact]
        public async Task DeleteQuestion_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            // Act
            var Question = await QuestionRepository.DeleteQuestion(Guid.NewGuid());
            // Assert
            Assert.Null(Question);
        }

        [Fact]
        public async Task GetQuestionFromText_WithValidText_ReturnsQuestion()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var ValidQuestion = new Question(ValidText, ValidAnswer, ValidDifficulty);
            await QuestionRepository.AddQuestion(ValidQuestion);
            // Act
            var Question = await QuestionRepository.GetQuestionFromText(ValidText);
            // Assert
            Assert.Equal(ValidQuestion, Question);
        }

        [Fact]
        public async Task GetQuestionFromText_WithInvalidText_ReturnsNull()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            // Act
            var Question = await QuestionRepository.GetQuestionFromText(ValidText);
            // Assert
            Assert.Null(Question);
        }
    }
}