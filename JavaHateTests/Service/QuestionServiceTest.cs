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
    public class QuestionServiceTest
    {
        private readonly string ValidText = "1 + 1";
        private readonly double ValidAnswer = 2;
        private readonly byte ValidDifficulty = 5;

        private MathMasterDBContext CreateFakeDbContext()
        {
            var options = new DbContextOptionsBuilder<MathMasterDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new MathMasterDBContext(options);
        }

        [Fact]
        public async Task AddQuestion_WithValidArguments_AddsQuestion()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var QuestionService = new QuestionService(QuestionRepository);
            var ValidQuestion = new QuestionCreateInput(ValidText, ValidAnswer, ValidDifficulty);
            // Act
            await QuestionService.AddQuestion(ValidQuestion);
            // Assert
            var Questions = await QuestionRepository.GetAllQuestions();
            Assert.Single(Questions);
        }

        [Fact]
        public async Task AddQuestion_WithDuplicateText_ThrowsIllegalArgumentException() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var QuestionService = new QuestionService(QuestionRepository);
            var ValidQuestion1 = new QuestionCreateInput(ValidText, ValidAnswer, ValidDifficulty);
            var ValidQuestion2 = new QuestionCreateInput(ValidText, 3, 7);
            await QuestionService.AddQuestion(ValidQuestion1);
            // Act & Assert
            IllegalArgumentException ex = await Assert.ThrowsAsync<IllegalArgumentException>(() => QuestionService.AddQuestion(ValidQuestion2));
            Assert.Equal("text", ex.Argument);
            Assert.Equal("A question with that text already exists", ex.Message);
        }

        [Fact]
        public async Task GetQuestionById_WithValidId_ReturnsQuestion()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var QuestionService = new QuestionService(QuestionRepository);
            var ValidQuestion = new QuestionCreateInput(ValidText, ValidAnswer, ValidDifficulty);
            var Question = await QuestionService.AddQuestion(ValidQuestion);
            // Act
            var RetrievedQuestion = await QuestionService.GetQuestionById(Question.Id);
            // Assert
            Assert.Equal(Question, RetrievedQuestion);
        }

        [Fact]
        public async Task GetQuestionById_WithInvalidId_ReturnsObjectNotFoundException()
        {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var QuestionService = new QuestionService(QuestionRepository);
            // Act & Assert
            ObjectNotFoundException ex = await Assert.ThrowsAsync<ObjectNotFoundException>(() => QuestionService.GetQuestionById(Guid.NewGuid()));
            Assert.Equal("question", ex.Object);
            Assert.Equal("No questions found with that ID", ex.Message);
        }

        [Fact]
        public async Task GetQuestion_WithUnspecifiedAmount_Returns1Question() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var QuestionService = new QuestionService(QuestionRepository);
            var ValidQuestion = new QuestionCreateInput(ValidText, ValidAnswer, ValidDifficulty);
            await QuestionService.AddQuestion(ValidQuestion);
            // Act
            var Questions = await QuestionService.GetQuestion();
            // Assert
            Assert.Single(Questions);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(12)]
        [InlineData(7)]
        public async Task GetQuestion_WithSpecifiedAmount_ReturnsSpecifiedAmountOfQuestions(int amount) {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var QuestionService = new QuestionService(QuestionRepository);
            for(int i = 0; i < amount + 7; i++) {
                string Text = $"{i} + {i}";
                double Answer = i + i;
                var ValidQuestion = new QuestionCreateInput(Text, Answer, ValidDifficulty);
                await QuestionService.AddQuestion(ValidQuestion);
            }
            // Act
            var Questions = await QuestionService.GetQuestion(amount);
            // Assert
            Assert.Equal(amount, Questions.Count);
        }

        [Fact]
        public async Task GetQuestion_WithNegativeAmount_ThrowsIllegalArgumentException() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var QuestionService = new QuestionService(QuestionRepository);
            await QuestionService.AddQuestion(new QuestionCreateInput(ValidText, ValidAnswer, ValidDifficulty));
            // Act & Assert
            IllegalArgumentException ex = await Assert.ThrowsAsync<IllegalArgumentException>(() => QuestionService.GetQuestion(-1));
            Assert.Equal("amount", ex.Argument);
            Assert.Equal("Amount must be greater than 0 (at least 1)", ex.Message);
        }

        [Fact]
        public async Task GetQuestion_WithEmptyDatabase_ThrowsObjectNotFoundException() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var QuestionService = new QuestionService(QuestionRepository);
            // Act & Assert
            ObjectNotFoundException ex = await Assert.ThrowsAsync<ObjectNotFoundException>(() => QuestionService.GetQuestion());
            Assert.Equal("question", ex.Object);
            Assert.Equal("No questions found", ex.Message);
        }

        [Fact]
        public async Task GetQuestion_WithEmptyDatabaseAndSpecifiedAmount_ThrowsObjectNotFoundException() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var QuestionService = new QuestionService(QuestionRepository);
            // Act & Assert
            ObjectNotFoundException ex = await Assert.ThrowsAsync<ObjectNotFoundException>(() => QuestionService.GetQuestion(5));
            Assert.Equal("question", ex.Object);
            Assert.Equal("No questions found", ex.Message);
        }

        [Fact]
        public async Task GetQuestion_WithEmptyDatabaseAndNegativeAmount_ThrowsObjectNotFoundException() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var QuestionService = new QuestionService(QuestionRepository);
            // Act & Assert
            ObjectNotFoundException ex = await Assert.ThrowsAsync<ObjectNotFoundException>(() => QuestionService.GetQuestion(-5));
            Assert.Equal("question", ex.Object);
            Assert.Equal("No questions found", ex.Message);
        }

        [Fact]
        public async Task UpdateQuestion_WithValidArguments_UpdatesQuestion() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var QuestionService = new QuestionService(QuestionRepository);
            var ValidQuestion = new QuestionCreateInput(ValidText, ValidAnswer, ValidDifficulty);
            var Question = await QuestionService.AddQuestion(ValidQuestion);
            Question.UpdateText("2 + 2");
            Question.UpdateAnswer(4d);
            // Act
            await QuestionService.UpdateQuestion(Question);
            var RetrievedQuestion = await QuestionService.GetQuestionById(Question.Id);
            // Assert
            Assert.Equal(Question.Text, RetrievedQuestion.Text);
            Assert.Equal(Question.Answer, RetrievedQuestion.Answer);
            Assert.Equal(Question.Difficulty, RetrievedQuestion.Difficulty);
        }

        [Fact]
        public async Task UpdateQuestion_WithInvalidId_ThrowsObjectNotFoundException() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var QuestionService = new QuestionService(QuestionRepository);
            var ValidQuestion = new QuestionCreateInput(ValidText, ValidAnswer, ValidDifficulty);
            var Question = await QuestionService.AddQuestion(ValidQuestion);
            var InvalidQuestion = new Question( "2 + 2", 4d, 1);
            // Act & Assert
            ObjectNotFoundException ex = await Assert.ThrowsAsync<ObjectNotFoundException>(() => QuestionService.UpdateQuestion(InvalidQuestion));
            Assert.Equal("question", ex.Object);
            Assert.Equal("No questions found with that ID", ex.Message);
        }

        [Fact]
        public async Task DeleteQuestion_WithValidId_DeletesQuestion() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var QuestionService = new QuestionService(QuestionRepository);
            var ValidQuestion = new QuestionCreateInput(ValidText, ValidAnswer, ValidDifficulty);
            var Question = await QuestionService.AddQuestion(ValidQuestion);
            // Act
            await QuestionService.DeleteQuestion(Question.Id);
            // Assert
            var Questions = await QuestionRepository.GetAllQuestions();
            Assert.Empty(Questions);
        }

        [Fact]
        public async Task DeleteQuestion_WithInvalidId_ThrowsObjectNotFoundException() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var QuestionService = new QuestionService(QuestionRepository);
            var ValidQuestion = new QuestionCreateInput(ValidText, ValidAnswer, ValidDifficulty);
            var Question = await QuestionService.AddQuestion(ValidQuestion);
            // Act & Assert
            ObjectNotFoundException ex = await Assert.ThrowsAsync<ObjectNotFoundException>(() => QuestionService.DeleteQuestion(Guid.NewGuid()));
            Assert.Equal("question", ex.Object);
            Assert.Equal("No questions found with that ID", ex.Message);
        }

        [Fact]
        public async Task DeleteQuestion_WithEmptyDatabase_ThrowsObjectNotFoundException() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var QuestionService = new QuestionService(QuestionRepository);
            // Act & Assert
            ObjectNotFoundException ex = await Assert.ThrowsAsync<ObjectNotFoundException>(() => QuestionService.DeleteQuestion(Guid.NewGuid()));
            Assert.Equal("question", ex.Object);
            Assert.Equal("No questions found with that ID", ex.Message);
        }

        [Fact]
        public async Task GetAllQuestions_WithEmptyDatabase_ThrowsObjectNotFoundException() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var QuestionService = new QuestionService(QuestionRepository);
            // Act & Assert
            ObjectNotFoundException ex = await Assert.ThrowsAsync<ObjectNotFoundException>(() => QuestionService.GetAllQuestions());
            Assert.Equal("question", ex.Object);
            Assert.Equal("No questions found", ex.Message);
        }

        [Fact]
        public async Task GetAllQuestions_WithQuestions_ReturnsQuestions() {
            // Arrange
            var FakeDbContext = CreateFakeDbContext();
            var QuestionRepository = new QuestionRepository(FakeDbContext);
            var QuestionService = new QuestionService(QuestionRepository);
            var ValidQuestion1 = new QuestionCreateInput(ValidText, ValidAnswer, ValidDifficulty);
            var ValidQuestion2 = new QuestionCreateInput("2 + 2", 4, 1);
            await QuestionService.AddQuestion(ValidQuestion1);
            await QuestionService.AddQuestion(ValidQuestion2);
            // Act
            var Questions = await QuestionService.GetAllQuestions();
            // Assert
            Assert.Equal(2, Questions.Count());
        }
    }
}